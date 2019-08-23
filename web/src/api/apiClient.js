import API from './api';

const getRemarks = function (details) {
  return API.get(`remarks`);
};

const addRemark = function (details) {
  return API.post(`remarks`, details);
};

const updateRemark = function (remarkId, remark) {
  return API.patch(`remarks/${remarkId}`, { remark });
};

const deleteRemark = function (remarkId) {
  return API.delete(`remarks/${remarkId}`);
};

export default {
  getRemarks,
  addRemark,
  updateRemark,
  deleteRemark
};