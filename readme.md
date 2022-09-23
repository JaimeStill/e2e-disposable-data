# Cypress Testing with Disposable Data API

> Still a work in progress.

* [Overview](#overview)
* [Relevant Infrastructure](#relevant-infrastructure)
* [Walkthroughs](#walkthroughs)
    * [Cypress Walkthrough](#cypress-walkthrough)
    * [Angular Rig Test Walkthrough](#angular-rig-test-walkthrough)
* [Notes](#notes)
    * [SQL Server Express](#sql-server-express)
    * [Cypress Configuration](#cypress-configuration)
    * [Cypress Asynchronous Tasks](#cypress-asynchronous-tasks)

## Overview
[Back to Top](#cypress-testing-with-disposable-data-api)

Given a .NET 6 API using EF Core with SQL Server and a web client, create an API-accessible server that allows a client service to initialize a disposable database and initialize an app API process pointed to the disposable database connection. I refer to this end to end infrastructure as a data rig. When executing Cypress tests, the app api and database can be initialized, and an initial data state can be established before all tests are run, then disposed of after all tests are completed.

The following demonstrates testing out the Rig API and client service via an Angular testing interface:  

https://user-images.githubusercontent.com/14102723/188249836-c5e70ac9-ba28-4851-8f1f-47cdebaea7e7.mp4

The following demonstrates using the Rig to:

* Before All:
    * Initialize a disposable database
    * Start a disposable app API process
    * Seed initial state data into the database
* Execute Cypress Tests
* After All:
    * Kill app API Process
    * Destroy disposable database

https://user-images.githubusercontent.com/14102723/190814410-96f3d5d9-3098-44b0-9d20-aa75142303c4.mp4

## Relevant Infrastructure
[Back to Top](#cypress-testing-with-disposable-data-api)

* [Rig Server](./server/Brainstorm.Rig)
    * [DbManager.cs](./server/Brainstorm.Data/DbManager.cs) - Connections are generated from [connections.json](./server/Brainstorm.Data/connections.json)
    * [ProcessRunner.cs](./server/Brainstorm.Rig/Services/ProcessRunner.cs)
    * [ApiRig.cs](./server/Brainstorm.Rig/Services/ApiRig.cs) - Registered as a Singleton service in [Program.cs](./server/Brainstorm.Rig/Program.cs#L34). Because it is created by the dependency injection container, it will properly execute the `IDisopsable.Dispose` method, cleaning up all session resources.
    * [RigController.cs](./server/Brainstorm.Rig/Controllers/RigController.cs)
* [Rig Client](./app/src/rig) - Written in native TypeScript to be framework agnostic
    * [rig.ts](./app/src/rig/rig.ts)
    * [rig-state.ts](./app/src/rig/rig-state.ts)
    * [rig-socket.ts](./app/src/rig/rig-socket.ts)
    * [rig-output.ts](./app/src/rig/rig-output.ts)    
* [test.route.ts](./app/src/brainstorm/app/routes/test/test.route.ts) - Facilitates Rig API interaction testing outside of the context of Cypress.
* [cypress](./app/src/brainstorm/cypress)
    * [test](./app/src/brainstorm/cypress/test/) - Defines route-specific tests and aggregates their execution under a single `Test` class.
        * [home.ts](./app/src/brainstorm/cypress/test/home.ts) - Defines functions for executing tests against 'http://localhost:3000', using the [Rig](./app/src/rig/rig.ts) client to generate / dispose data state.
        * [index.ts](./app/src/brainstorm/cypress/test/index.ts) - Exposes a `Test` class that provides methods pointing to the `test` method of each internal test class.
    * [e2e](./app/src/brainstorm/cypress/e2e) - Cypress test root directory
        * [home.cy.ts](./app/src/brainstorm/cypress/e2e/home.cy.ts) - Executes `Test.home()` as defined in the [test](./app/src/brainstorm/cypress/test/index.ts) directory.

## Walkthroughs
[Back to Top](#cypress-testing-with-disposable-data-api)

Before executing, make sure to install dependencies and build:

```bash
cd /server/
dotnet build

cd ../app/
npm i
npm run build
```

Also be sure to see the [SQL Server Express](#sql-server-express) section.

Connection strings are defined at:

* [appsettings.Development.json](./server/Brainstorm.Api/appsettings.Development.json)
* [connections.json](./server/Brainstorm.Data/connections.json)

URLs:

Asset | URL | Start
------|-----|------
[Brainstorm.Api](./server/Brainstorm.Api/) | http://localhost:5000 | `/server/Brainstorm.Api > dotnet run`
[Brainstorm.Rig](./server/Brainstorm.Rig/) | http://localhost:5001 | `/server/Brainstorm.Rig > dotnet run`
[Angular App](./app/src/brainstorm) | http://localhost:3000 | `/app > npm run start`

### Cypress Walkthrough
[Back to Top](#cypress-testing-with-disposable-data-api)

In VS Code, open three different terminals:

**Terminal 1**

```bash
cd /server/Brainstorm.Rig/
dotnet run
```

**Terminal 2**

```bash
cd /app/
npm run start
```

**Terminal 3**

```bash
cd /app/
npm run e2e-open
```

This will launch the Cypress testing app. Click **E2E Testing**:

![image](https://user-images.githubusercontent.com/14102723/191865299-77080a58-3acd-4a0b-a85b-84684cd90e65.png)

Select your browser of choice and click **Start E2E Testing**:

![image](https://user-images.githubusercontent.com/14102723/191867070-1e850729-5ba5-48e4-b390-079e04acd13a.png)

This will load a test browser with any defined Cypress tests. Click **home** to begin executing the home test:  

![image](https://user-images.githubusercontent.com/14102723/191869049-0bc8be90-0188-4808-956f-6d1185e48e3d.png)

![image](https://user-images.githubusercontent.com/14102723/191869302-054d4b08-7fd0-4b0f-b86a-42fefabacd20.png)


### Angular Rig Test Walkthrough
[Back to Top](#cypress-testing-with-disposable-data-api)

> Screenshots / videos still need to be generated.

In VS Code, open two different terminals:

**Terminal 1**

```bash
cd /server/Brainstorm.Rig/
dotnet run
```

**Terminal 2**

```bash
cd /app/
npm run start
```

The available Rig API endpoints can be found at http://localhost:5001/swagger :

![image](https://user-images.githubusercontent.com/14102723/191869783-9ff9194b-ef7c-41c0-b27e-a817d74db340.png)


From a browser, navigate to http://localhost:3000/test :

![image](https://user-images.githubusercontent.com/14102723/191869844-8ff87ecc-7d8e-43c5-9b7e-251efd630811.png)

Clicking the **Initialize Database** toggle will cause an database instance associated with the displayed connection string to be generated:

![image](https://user-images.githubusercontent.com/14102723/191869947-83e7fb55-2488-48b6-a0ef-69db8224661e.png)

You can verify the database in SQL Server Management Studio:

![image](https://user-images.githubusercontent.com/14102723/191870985-ef9b2243-f716-4ff9-ae1c-fb62277cb608.png)

Clicking the **Start Process** toggle will cause a disposable app API process pointed to the disposable database to be initialized:

![image](https://user-images.githubusercontent.com/14102723/191870043-e5ba9453-96f7-428f-810a-cb0a754e5f7c.png)

While this process is runnning, the app API is available. From a separate tab, you can see the endpoints at http://localhost:5000/swagger :

![image](https://user-images.githubusercontent.com/14102723/191870175-178d7848-929d-4984-960d-b2f9642a5a15.png)

This should also enable the **Seed Topic** button at the test route to be enabled. Clicking it will send the JSON `Topic` object to the Rig's `SeedTopic` endpoint and update the displayed JSON with the returned object:

![image](https://user-images.githubusercontent.com/14102723/191870301-8664a33b-091a-43fc-8961-2fe4671a1710.png)

Additionally, more data can be added to the database via the provided topic form:

![image](https://user-images.githubusercontent.com/14102723/191870536-5bf0a7b7-d3e2-4cca-a516-4c6dbfab9df5.png)

After clicking **Save Topic**, you can validate the data that has been saved by navigating to http://localhost:3000 in a new tab:

![image](https://user-images.githubusercontent.com/14102723/191870649-609f5246-f317-44bd-81c6-4a6ac9791a60.png)

In the test route tab, clicking **Kill Process** will dispose the app API server process and API calls will no longer be available:

![image](https://user-images.githubusercontent.com/14102723/191870792-c34e069d-862c-4e74-a711-ea80779096ee.png)

You can verify this by attempting to navigate back to http://localhost:5000/swagger in a new tab:

![image](https://user-images.githubusercontent.com/14102723/191870862-16aa39e7-4402-4f70-98f4-4ddbbec73729.png)

In the test route, clicking the **Destroy Database** toggle will cause the database to be destroyed:

![image](https://user-images.githubusercontent.com/14102723/191871138-0f0552c4-c9d6-41f9-aa56-e3d9c6bbc96a.png)

You can verify this by refreshing the *Databases* directory in SQL Server Management Studio:

![image](https://user-images.githubusercontent.com/14102723/191871196-76bc0e62-ea05-4f58-9a03-ba29806834da.png)

It's also worth noting that you can initialize a new database and start a new process. Navigating back to the test route will keep track of the process and database state, but not the seeded topic. You can seed the same topic multiple times by refreshing the tab.

Additionally, stopping the Rig server process will cause the app API process to be disposed and the database destroyed:

![image](https://user-images.githubusercontent.com/14102723/191873166-fae635ff-febd-4035-8b80-b5739eb90c0f.png)  

## Notes
[Back to Top](#cypress-testing-with-disposable-data-api)

### SQL Server Express
[Back to Top](#cypress-testing-with-disposable-data-api)

Testing environment runs using [SQL Server 2019 Express](https://go.microsoft.com/fwlink/p/?linkid=866658) with the server name of `DevSql` and Windows authentication.

In SQL Server Management Studio, right-click the server in object explorer and click **Properties**:

* In the **Security** tab, *cross database ownership chaining* is enabled:

    ![image](https://user-images.githubusercontent.com/14102723/190693425-b43870c4-260f-4959-846f-9fb9834972a9.png)

* In the **Advanced** tab, *Enable Contained Databases* is set to `True`:

    ![image](https://user-images.githubusercontent.com/14102723/190814946-7fa19572-7429-42ff-9539-f2c17f2d4382.png)

Additional Links:
* [SQL Server Management Studio](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16)
* [SQL Server 2019 Cumulative Update](https://support.microsoft.com/en-us/topic/kb5016394-cumulative-update-17-for-sql-server-2019-3033f654-b09d-41aa-8e49-e9d0c353c5f7)

### Cypress Configuration

[**cypress.config.ts**](./app/src/brainstorm/cypress.config.ts)

* `baseUrl: http://localhost:3000` - Required to prevent re-calling the `before` and `after` methods when `cy.visit()` is executed.

* `defaultCommandTimeout: 8000` - Give a larger buffer for API method execution, particularly when executing `rig.startProcess()`.

[**tsconfig.json**](./app/src/brainstorm/cypress/tsconfig.json)

The following settings are required to be able to use the external [Rig](./app/src/rig/rig.ts) client within tests:

```json
{
    "compilerOptions": {
        "target": "ES5",
        "lib": ["ES5", "DOM"],
        "types": [
            "cypress",
            "node"
        ]
    }
}
```

### Cypress Asynchronous Tasks
[Back to Top](#cypress-testing-with-disposable-data-api)

Cypress does not support using [async / await in tests](https://github.com/cypress-io/cypress/issues/1417) and internally handles asynchrony in non-standard ways. Because of this, when executing asyncronous tasks with non-Cypress infrastructure, these calls have to be wrapped in `cy.then(() => )` and resolved with its own `.then(() => )` call. For instance, when initializing the database before executing tests, the following structure must be used within a test:

```ts
// before all tests execute
before(() => {
    // initialize an asynchronous action with Cypress
    cy.then(() =>
        // execute an external Promise
        rig.initializeDatabase().then(() =>
            // log once the promise completes.
            cy.log('Database initialized')
        )
    );
})
```
