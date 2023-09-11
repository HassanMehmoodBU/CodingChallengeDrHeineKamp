
# Coding Challenge Dr Heinekamp

The application is a document library intended to give its users a web based solution to store and share their documents with others.

## Run Locally

To run this project Locally

Clone the project

```bash
  git clone https://github.com/HassanMehmoodBU/CodingChallengeDrHeineKamp.git
```

### Back-End

- Open BackEnd.sln and open FileServerDatabase and run FileServerDatabase.publish
- Create an empty database and set the connection string and empty database to the publish profile editor.
- Publish Database and verify
- Update the connection string accordingly in Web Api project "BackEnd" appsettings.json
- Run the app with "BackEnd" Debug Properties if you wish to Debug or simply Deploy

### Front-End

- Go to folder FrontEnd/Front-End
- First install all dependencies via terminal

```bash
  npm install
```

- Simply run 

```bash
  npm start
```


## How To

- Click on Register from Menu
- Create a User
- Login with the credentials
- Click on Upload from Menu
- Select File(s) to upload
- In the list below click on any file to download
- In the list below select multiple files and then click on Download button to download multiple files.
- In the list below click on ShareAble Link to open a popup.
- Enter number of days and hours you want to allow this file to be shareable.
- Once you click Generate Link button it will automatically show you a public link which can be used for the specified time period.
- If the file already has a shareable link it will be shown on popup.


