import axios from 'axios';

import config from '@src/config';
import session from '@src/session';

export default axios.create({
  baseURL: config.API_URL,
  headers: {
    'X-Requested-With': 'XMLHttpRequest',
    'Authorization': `Bearer ${session.token}`
  },
});