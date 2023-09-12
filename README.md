
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


## Deliverables:

- Code and other assets access - zip archive, github / gitlab repository (provided via email)
- Instructions how to run & test the application (provided above)
- Description of main architecture and design decisions

- Web API .Net 6 (back-end)
- JwtMiddleWare for Token Authentication with ASP Identity
- Typical CQRS pattern without event hadler instead used simple Providers for data retreival and Managers for data posting.
- Use of Dependency Injection
- SQL Project for easy schema deployment
- Entity Framework for smooth Data queries.
- Angular (front-end)

## Ideas and proposals how to improve the application from a user or technical perspective

- With more time invest and having a bigger scope the application architecture can be made alot different.
- can create a custom Identity Server with ASP Identity to handle everything to have less dependency
- can use proper CQRS implementation.
- if the scope is big can be used as a micro-service.
- can make DAL separate from the Logic/Shared Library.