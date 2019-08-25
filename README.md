# Landmark Remark Application

Repository for the APP (ReactJS) and API (.NET Core) projects of the Landmark Remark application.

## Initialise both projects:
    > init.cmd

In this command, the package dependencies of both projects will be retrieved.

## Run tests on both projects:
    > test.cmd

In this command, both the project's test suites will be executed.

## Run the application:
    > start.cmd

In this command, each project will be built and published to the same output location so they are run as a single application.

NOTE: On Visual Studio Code, you can also run the application by pressing F5.

## Run the projects on separate processes (for development):
    > dev-start.cmd

In this command, both projects will run on their own process. The APP is configured to allow hot loading of source files for rapid development and easy troubleshooting. A debugger can be attached to the API process for easy troubleshooting.

However for this to work, the APP is hardcoded to talk to the API on https://localhost:5001.