export interface Document {
    DocumentId: string;
    DocumentName: string;
    DocumentType: string;
    UploadDateTime: Date;
    NoOfDownloads: Number;
    IsShareAble: boolean;
    ShareAbleLink: string;
    ShareExpiry: Date;
    UploadedBy: string;
}