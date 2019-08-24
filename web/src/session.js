const getUser = () => {
  const isLoggedIn = window.sessionStorage.getItem('user.isLoggedIn') == 'true';
  const idToken = window.sessionStorage.getItem('user.idToken');
  const email = window.sessionStorage.getItem('user.email');

  return {
    isLoggedIn,
    idToken,
    email
  }
};

const start = (data) => {
  const user = {
    isLoggedIn: data != null,
    idToken: data.idToken,
    email: data.email
  };

  window.sessionStorage.setItem('user.isLoggedIn', user.isLoggedIn);
  window.sessionStorage.setItem('user.idToken', user.idToken);
  window.sessionStorage.setItem('user.email', user.email);

  return user;
};

const stop = () => {
  window.sessionStorage.clear();
};

export default {
  getUser,
  start,
  stop
};