# Cypress Testing with Data Seeding Service

> Still a work in progress.

Given a .NET 6 API and an Angular app, create an API-accessible server that allows a client service to initialize data state from a client service and run the API server pointed to this data to facilitate Cypress tests.

https://user-images.githubusercontent.com/14102723/188249836-c5e70ac9-ba28-4851-8f1f-47cdebaea7e7.mp4

## Current Features

1. Initialization of a unique test database instance:
    * Generates a unique connection string and applies all migrations to the target database instance.

    * Ability to destroy and re-initialize with a new database.

2. Start the API server project in a managed process.

3. When the server process ends, kill the API server process and destroy the test database.

Relevant Infrastructure:

* [DbManager.cs](./server/Brainstorm.Data/DbManager.cs)
* [ProcessRunner.cs](./server/Brainstorm.Rig/Services/ProcessRunner.cs)
* [ApiRig.cs](./server/Brainstorm.Rig/Services/ApiRig.cs)
    * Registered as a Singleton service in [Program.cs](./server/Brainstorm.Rig/Program.cs#L34). Because it is created by the dependency injection container, it will properly execute the `IDisopsable.Dispose` method, cleaning up all session resources.
* [RigController.cs](./server/Brainstorm.Rig/Controllers/RigController.cs)

## Walkthrough

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
