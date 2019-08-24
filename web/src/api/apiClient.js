import API from './api';

const register = function (details) {
  return API.init().post(`user/register`, details);
};

const login = function (details) {
  return API.init().post(`user/login`, details);
};

const getRemarks = function (details) {
  return API.init().get(`remarks`);
};

const addRemark = function (details) {
  return API.init().post(`remarks`, details);
};

const updateRemark = function (remarkId, remark) {
  return API.init().patch(`remarks/${remarkId}`, { remark });
};

const deleteRemark = function (remarkId) {
  return API.init().delete(`remarks/${remarkId}`);
};

export default {
  register,
  login,
  getRemarks,
  addRemark,
  updateRemark,
  deleteRemark
};