# Landmark Remark

## Requirements
1. As a user (of the application) I can see my current location on a map
2. As a user I can save a short note at my current location
3. As a user I can see notes that I have saved at the location they were saved  on the map 
4. As a user I can see the location, text, and user-name of notes other users  have saved
5. As a user I have the ability to search for a note based on contained text or  user-name


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