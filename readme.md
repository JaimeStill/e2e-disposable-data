# Cypress Testing with Disposable Data API

> Still a work in progress.

* [Overview](#overview)
* [Relevant Infrastructure](#relevant-infrastructure)
* [Rig Server API Walkthrough](#rig-server-api-walkthrough)
* [Notes](#notes)
    * [SQL Server Express](#sql-server-express)

## Overview
[Back to Top](#cypress-testing-with-disposable-data-api)

Given a .NET 6 API and an Angular app, create an API-accessible server that allows a client service to initialize disposable data state from a client service. When executing Cypress tests, the data state can be initialized before all tests are run, then disposed of after all tests are completed.

https://user-images.githubusercontent.com/14102723/188249836-c5e70ac9-ba28-4851-8f1f-47cdebaea7e7.mp4

https://user-images.githubusercontent.com/14102723/190687828-46580be2-e3ad-4a99-b710-f5032d126711.mp4

## Relevant Infrastructure
[Back to Top](#cypress-testing-with-disposable-data-api)

* [Rig Server](./server/Brainstorm.Rig)
    * [DbManager.cs](./server/Brainstorm.Data/DbManager.cs)
        * Connections are generated from [connections.json](./server/Brainstorm.Data/connections.json)
    * [ProcessRunner.cs](./server/Brainstorm.Rig/Services/ProcessRunner.cs)
    * [ApiRig.cs](./server/Brainstorm.Rig/Services/ApiRig.cs)
        * Registered as a Singleton service in [Program.cs](./server/Brainstorm.Rig/Program.cs#L34). Because it is created by the dependency injection container, it will properly execute the `IDisopsable.Dispose` method, cleaning up all session resources.
    * [RigController.cs](./server/Brainstorm.Rig/Controllers/RigController.cs)
* [Rig Client](./app/src/rig) - Written in native TypeScript to be framework agnostic
    * [rig.ts](./app/src/rig/rig.ts)
    * [rig-state.ts](./app/src/rig/rig-state.ts)
    * [rig-socket.ts](./app/src/rig/rig-socket.ts)
    * [rig-output.ts](./app/src/rig/rig-output.ts)    
* [test.route.ts](./app/src/brainstorm/app/routes/test/test.route.ts)
    * Facilitates Rig API interaction testing outside of the context of Cypress.
* [home.cy.ts](./app/src/brainstorm/cypress/e2e/home.cy.ts)
    * Executes tests against `http://localhost:3000`, using the [Rig](./app/src/rig/rig.ts) client to generate / dispose data state.

## Rig Server API Walkthrough
[Back to Top](#cypress-testing-with-disposable-data-api)

1. Starting up the [Brainstorm.Rig](./server/Brainstorm.Rig) project gives access to the API:  

    ![01-dotnet-run](https://user-images.githubusercontent.com/14102723/185768597-417e5772-4f65-48f7-8d88-29f79b054ce3.png)

2. Calling the `InitializeDatabase` API method generates the test database:

    ![02-initialize-database](https://user-images.githubusercontent.com/14102723/185768718-94114c04-75d1-44dd-94df-58211a09f2ba.png)

3. Calling the `StartProcess` API method starts the [Brainstorm.Api](./server/Brainstorm.Api) project pointed to the test connection string:

    ![03-start-process](https://user-images.githubusercontent.com/14102723/185768726-bd39b80e-d31e-44a4-aff9-72e467c7ad8e.png)

4. The [Brainstorm.Api](./server/Brainstorm.Api) project is now available to interact with:

    ![04-api-swagger](https://user-images.githubusercontent.com/14102723/185768760-a2a06e10-6155-43ea-865b-7c11d889eca8.png)

5. Example of adding a Topic:

    ![05-api-post](https://user-images.githubusercontent.com/14102723/185768775-c7f986f3-86a0-49c1-a6b6-fc309e9eb086.png)

6. Example of calling the Topic `Query` endpoint:

    ![06-api-query](https://user-images.githubusercontent.com/14102723/185768787-ecd435a3-6f6f-43c2-800a-f7f723eb11ac.png)

7. When the Rig server process ends, it properly disposes of the Api server process and deletes the test database:

    ![07-end-session](https://user-images.githubusercontent.com/14102723/185768797-09ff7527-53a8-4bb4-8c74-fca5d47a969c.png)

## Notes
[Back to Top](#cypress-testing-with-disposable-data-api)

### SQL Server Express
[Back to Top](#cypress-testing-with-disposable-data-api)

Testing environment runs using [SQL Server 2019 Express](https://go.microsoft.com/fwlink/p/?linkid=866658) with the server name of `DevSql`.

Additionally, in SQL Server Management Studio, *cross database ownership chaining* is enabled in *Server Properties (right-click the server and click **Properties**) > Security tab > Options section*.

Additional Links:
* [SQL Server Management Studio](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16)
* [SQL Server 2019 Cumulative Update](https://support.microsoft.com/en-us/topic/kb5016394-cumulative-update-17-for-sql-server-2019-3033f654-b09d-41aa-8e49-e9d0c353c5f7)

