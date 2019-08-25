const save = (option, value) =>
  window.localStorage.setItem(`landmark-remark.${option}`, value);

const get = (option) =>
  window.localStorage.getItem(`landmark-remark.${option}`);

export default {
  save,
  get
};