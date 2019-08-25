# Landmark Remark

## Requirements
1. As a user (of the application) I can see my current location on a map
2. As a user I can save a short note at my current location
3. As a user I can see notes that I have saved at the location they were saved  on the map 
4. As a user I can see the location, text, and user-name of notes other users  have saved
5. As a user I have the ability to search for a note based on contained text or  user-name

## Implicit Requirements
1. Users needs to be differentiated from each other.
    * User should be able to register in the app.
    * User should need to login in the app.
2. A user can add, update or delete notes.
    * User should only be able to read other user's notes.
3. User should be able to navigate between notes easily.
4. User should be able to navigate back to the current location easily.
5. User should be able to add notes anywhere (not just in the current location).

## Approach
The approach was to create a single page app in ReactJS which talks to a .NET Core web API backend. For user authentication, Firebase will be used; and Firestore for the data storage.

The backend will provide the authentication end point as well as the search, add, edit, and update operations. The app will handle the presentation of the data. The two will be on separate projects but will both publish on the same shared location.

The backend was built first since it has no dependency on the app. Most of the time spent on the backend is on working with the Firestore queries and JSON format. The app was then built after the backend to avoid mocking API responses during development. Most of the time spent on the app is on the initial project setup as well as working with the google-maps-react component.

The whole application was built at around 35 hours broken down roughly as follows:

|Task                     | Hours spent |
|-------------------------|-------------|
| **Backend development** |   **12**    |
| > Project setup         |      2      |
| > Coding & Test         |      7      |
| > Research              |      2      |
| > Troubleshooting       |      1      |
| **App development**     |   **19**    |
| > Project setup         |      2      |
| > Coding & Test         |      6      |
| > Research              |      4      |
| > Troubleshooting       |      7      |
| **Polish**              |    **3**    |
| **Documentation**       |    **1**    |


## Known issues or limitations
* Firebase setup is on free tier and so is bound by the specified limits for that tier. Same goes for the Google API access.
* The app has no field validations and just displays the error message from the API.
* The app does not dynamically load the notes depending on the map region currently on the screen.
* The app does not redirect to the login screen on session expiry. It just displays the error coming from the API.
* The app is using sessionStorage for storing the auth token, which is cleared when current browser tab is closed.
* There is no list view for all the notes currently on display. The user has to navigate the map or go through each note one by one.
* The backend calls Firestore via REST to avoid the overhead of working with the Firebase SDK. However, the backend just reuses the auth token sent by the app when communicating with Firestore.
* The backend has the database details and Firebase API keys on the config.
* There is no logging in the backend.