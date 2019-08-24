import axios from 'axios';

import config from '@config';
import session from '@src/session';

const init = () => {
  const headers = { 'X-Requested-With': 'XMLHttpRequest' };
  const user = session.getUser();

  if(user.isLoggedIn) {
    headers['Authorization'] = `Bearer ${user.idToken}`;
  }

  return axios.create({
    baseURL: config.API_URL,
    headers
  });
};

export default {
  init
};