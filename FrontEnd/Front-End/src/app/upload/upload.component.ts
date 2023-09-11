import { HttpClient, HttpEventType, HttpRequest, HttpHeaders, HttpResponse } from '@angular/common/http';
import { RepositoryService } from './../shared/services/repository.service';
import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Router } from '@angular/router';
import { Document } from './../_interfaces/document.model';
import { EnvironmentUrlService } from './../shared/services/environment-url.service';

@Component({
    selector: 'app-upload',
    templateUrl: './upload.component.html',
    styleUrls: ['./upload.component.scss'],
})
export class UploadComponent implements OnInit {
    private returnUrl: string;
    showError: boolean;
    errorMessage: string = '';

    constructor(private http: HttpClient, private router: Router,
        private repository: RepositoryService, private envUrl: EnvironmentUrlService) { }

    working = false;
    uploadFiles: File[] = [];
    uploadFileLabel: string | undefined = 'Choose File(s) to upload';
    uploadProgress: number;
    uploadUrls: string[];
    doucments: Document[];
    selectedDocumentIds: string[] = [];
    showPopup = false;
    inputDays: number = 0;
    inputHours: number = 0;
    shareAbleFileName = '';
    shareAbleDocumentId = '';
    ShareableLinkText = 'Link Unavailable';
    ShareExpiry: Date = null;

    iconList = [
        { type: "xlsx", icon: "bi-file-excel" },
        { type: "xls", icon: "bi-file-excel" },
        { type: "pdf", icon: "bi-file-pdf" },
        { type: "jpg", icon: "bi-file-image" },
        { type: "doc", icon: "bi-file-word" },
        { type: "docx", icon: "bi-file-word" },
        { type: "txt", icon: "bi-file-text" }
    ];


    ngOnInit(): void {
        var token = localStorage.getItem("token");

        if (!token) {
            this.router.navigate(["authentication/login"]);
        }

        this.getAllDocs();
    }

    getFileExtension(filename) {
        let ext = filename.split(".").pop();
        let obj = this.iconList.filter(row => {
            if (row.type === ext) {
                return true;
            }
        });
        if (obj.length > 0) {
            let icon = obj[0].icon;
            return icon;
        } else {
            return "";
        }
    }

    saveData() {

        const authToken = localStorage.getItem("token");;

        const headers_data = new HttpHeaders({
            'Authorization': `Bearer ${authToken}`
        });

        this.repository.generateShareAbleLink(headers_data, this.shareAbleDocumentId, this.inputDays, this.inputHours).subscribe(
            (response) => {
                this.ShareableLinkText = this.envUrl.urlAddress + '/api/documents/downloadlink/' + response.documentId;
                this.ShareExpiry = new Date(response.shareExpiry);
            },
            (error) => {
                console.error('Error generating link:', error);
            }
        );
    }

    openPopup() {
        this.showPopup = true;
    }

    cancelPopup() {
        this.shareAbleFileName = '';
        this.ShareableLinkText = 'Link Unavailable';
        this.shareAbleDocumentId = '';
        this.ShareExpiry = null;
        this.showPopup = false;
    }

    handleFileInput(files: FileList | null) {
        this.invalidateErrorMessage();
        this.clearFiles();
        const allowedTypes = [
            'application/pdf',
            'application/vnd.ms-excel',
            'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
            'application/msword',
            'application/vnd.openxmlformats-officedocument.wordprocessingml.document',
            'text/plain',
            'image/jpeg',
            'image/png',
            'image/gif',
        ];
        if (files && files.length > 0) {
            for (let i = 0; i < files.length; i++) {
                if (!allowedTypes.includes(files[i].type)) {
                    this.errorMessage = "Please select only allowed file types!";
                    this.showError = true;
                    this.clearFiles();
                    return;
                }
                this.uploadFiles.push(files[i]);
            }

            this.uploadFileLabel = `${files.length} ${files.length > 1 ? 'Files' : 'File'} selected`;
        }
    }

    upload() {
        this.invalidateErrorMessage();

        if (this.uploadFiles.length === 0) {
            this.errorMessage = "Please select at least one file to upload!";
            this.showError = true;
            return;
        }

        const formData = new FormData();

        for (const file of this.uploadFiles) {
            formData.append(file.name, file);
        }
        const authToken = localStorage.getItem("token");;

        const headers_data = new HttpHeaders({
            'Authorization': `Bearer ${authToken}`
        });

        const url = `${environment.urlAddress}/api/documents/upload`;
        const uploadReq = new HttpRequest('POST', url, formData, {
            reportProgress: true,
            headers: headers_data
        });

        this.uploadUrls = []; // Array to store uploaded file URLs
        this.uploadProgress = 0;
        this.working = true;

        this.http.request(uploadReq).subscribe((event: any) => {
            if (event.type === HttpEventType.UploadProgress) {
                this.uploadProgress = Math.round((100 * event.loaded) / event.total);
            } else if (event.type === HttpEventType.Response) {
                if (Array.isArray(event.body.urls)) {
                    this.uploadUrls = event.body.urls;
                } else {
                    this.uploadUrls = [event.body.urls];
                }
            }
        }, (error: any) => {
            console.error(error);
        }).add(() => {
            this.working = false;
            this.clearFiles();
            this.clearDocs();
            this.getAllDocs();

        });
    }

    invalidateErrorMessage() {
        this.errorMessage = "";
        this.showError = false;
    }

    clearFiles() {
        this.uploadFiles = [];
        this.uploadFileLabel = "";
    }

    clearDocs() {
        this.doucments = [];
    }

    getAllDocs = () => {
        const authToken = localStorage.getItem("token");;

        const headers_data = new HttpHeaders({
            'Authorization': `Bearer ${authToken}`
        });

        this.repository.getAllFiles(headers_data).subscribe(
            (response) => {
                this.doucments = response;
            },
            (error) => {
                console.error('Error fetching files:', error);
            }
        );


    }

    downloadDocument(fileGuid: string): void {
        const authToken = localStorage.getItem("token");;

        const headers_data = new HttpHeaders({
            'Authorization': `Bearer ${authToken}`
        });

        this.repository.downloadFile(headers_data, fileGuid).subscribe(
            (response: HttpResponse<Blob>) => {
                const contentDisposition = response.headers.get('Content-Disposition');
                let fileName = 'downloadedFile.ext'; // Default filename

                if (contentDisposition) {
                    const matches = contentDisposition.match(/filename="?([^"]+)"?/);
                    if (matches && matches.length > 1) {
                        fileName = matches[1]?.split(";")[0];
                    }
                }
                const blobURL = window.URL.createObjectURL(response.body);
                const anchor = document.createElement('a');
                anchor.href = blobURL;
                anchor.download = fileName
                anchor.style.display = 'none';
                document.body.appendChild(anchor);
                anchor.click();
                window.URL.revokeObjectURL(blobURL);
                document.body.removeChild(anchor);
            },
            (error) => {
                console.error('Error downloading file:', error);
            },
            () => {
                this.router.navigate(["/upload"]);
            }
        );
    }

    onCheckboxChange(event: any, documentId: string): void {
        if (event.target.checked) {
            this.selectedDocumentIds.push(documentId);
        } else {
            const index = this.selectedDocumentIds.indexOf(documentId);
            if (index !== -1) {
                this.selectedDocumentIds.splice(index, 1);
            }
        }
    }

    downloadSelectedDocuments() {
        if (this.areItemsSelected) {

            const authToken = localStorage.getItem("token");;

            const headers_data = new HttpHeaders({
                'Authorization': `Bearer ${authToken}`
            });

            this.repository.downloadMultipleFiles(headers_data, this.selectedDocumentIds).subscribe(
                (response: Blob) => {
                    const blobURL = window.URL.createObjectURL(response);
                    const anchor = document.createElement('a');
                    anchor.href = blobURL;
                    anchor.download = 'downloadedFiles.zip';
                    anchor.style.display = 'none';
                    document.body.appendChild(anchor);
                    anchor.click();
                    window.URL.revokeObjectURL(blobURL);
                    document.body.removeChild(anchor);
                },
                (error) => {
                    console.error('Error downloading file:', error);
                },
                () => {
                    this.router.navigate(["/upload"]);
                }
            );
        }
    }

    shareAbleLink(documentId: string, documentName: string, expiry: Date) {
        this.shareAbleDocumentId = documentId;
        this.shareAbleFileName = documentName;
        const currentDate = new Date();
        const specificDate = new Date(expiry);
        this.ShareExpiry = specificDate;
        if (this.ShareExpiry != null && specificDate >= currentDate) {
            this.ShareableLinkText = this.envUrl.urlAddress + '/api/documents/downloadlink/' + this.shareAbleDocumentId;
        }
        this.openPopup()
    }

    areItemsSelected(): boolean {
        return this.selectedDocumentIds.length > 0;
    }

}
