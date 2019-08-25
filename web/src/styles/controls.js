import styled from 'styled-components';

const Wrapper = styled.div`
  font-size: 16px;
  display: flex;
  flex-direction: column;
  align-items: stretch;
  min-width: 200px;
`;

const Header = styled.div`
  font-weight: bold;
  padding: 1vh 1vw;
`;

const Footer = styled.span`
  padding: 1vh 1vw;
  margin: 1vh 1vw;
  font-size: smaller;
  color: ${props => getStatusColor(props)};
  align-self: flex-start;
  display: ${props => getStatusDisplay(props)};
`;


const Status = styled.span`
  padding: 1vh 1vw 1vh 0px;
  margin: 1vh 1vw 1vh 0px;
  color: ${props => getStatusColor(props)};
  align-self: flex-start;
  display: ${props => getStatusDisplay(props)};
`;

const getStatusColor = (props) => {
  if(props.error) { return 'red'; }
  return 'inherit';
};

const getStatusDisplay = (props) => {
  if(props.show) { return 'flex'; }
  return 'none';
};

const FieldWrapper = styled.div`
  display: flex;
  justify-content: flex-start;
  flex-wrap: wrap;
`;

const Label = styled.div`
  padding: 1vh 1vw;
  align-self: center;
  min-width: 80px;
  width: 10vw;
`;

const ReadOnly = styled.div`
  font-size: larger;
  padding: 1vh 1vw;
`;

const Input = styled.input`
  font-size: larger;
  padding: 1vh 1vw;
  margin: 1vh 1vw;
  border: 1px solid;
`;

const EmailValidationIcon = styled.span`
  background-image: url(${props => getEmailIcon(props.value)});
  width: 30px;
  height: 30px;
  align-self: center;
  background-repeat: no-repeat;
  background-size: contain;
`;

const getEmailIcon = (value) => {
  if(!value) { return ''; }

  if(/^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/.test(value)) {
    return 'https://img.icons8.com/cotton/64/000000/checkmark.png';
  }

  return 'https://img.icons8.com/cotton/64/000000/delete-sign--v2.png';
}

const PasswordStrengthIcon = styled.span`
  background-image: url(${props => getStrengthIcon(props.value)});
  width: 30px;
  height: 30px;
  align-self: center;
  background-repeat: no-repeat;
  background-size: contain;
`;

const getStrengthIcon = (value) => {
  if(!value) { return ''; }
  if(value.length < 6) { return 'https://img.icons8.com/cotton/64/000000/delete-sign--v2.png'; }

  let strength = 0;
  if(/[A-Z]/.test(value)) { strength++; }
  if(/[a-z]/.test(value)) { strength++; }
  if(/[0-9]/.test(value)) { strength++; }
  if(/[^A-Za-z0-9]/.test(value)) { strength++; }
  if(value.length >= 8) { strength++; }

  if(strength <= 3) { return 'data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHg9IjBweCIgeT0iMHB4Igp3aWR0aD0iNjQiIGhlaWdodD0iNjQiCnZpZXdCb3g9IjAgMCAxNzIgMTcyIgpzdHlsZT0iIGZpbGw6IzAwMDAwMDsiPjxnIGZpbGw9Im5vbmUiIGZpbGwtcnVsZT0ibm9uemVybyIgc3Ryb2tlPSJub25lIiBzdHJva2Utd2lkdGg9Im5vbmUiIHN0cm9rZS1saW5lY2FwPSJub25lIiBzdHJva2UtbGluZWpvaW49Im1pdGVyIiBzdHJva2UtbWl0ZXJsaW1pdD0iMTAiIHN0cm9rZS1kYXNoYXJyYXk9IiIgc3Ryb2tlLWRhc2hvZmZzZXQ9IjAiIGZvbnQtZmFtaWx5PSJub25lIiBmb250LXdlaWdodD0ibm9uZSIgZm9udC1zaXplPSJub25lIiB0ZXh0LWFuY2hvcj0ibm9uZSIgc3R5bGU9Im1peC1ibGVuZC1tb2RlOiBub3JtYWwiPjxwYXRoIGQ9Ik0wLDE3MnYtMTcyaDE3MnYxNzJ6IiBmaWxsPSJub25lIiBzdHJva2U9Im5vbmUiIHN0cm9rZS13aWR0aD0iMSIgc3Ryb2tlLWxpbmVjYXA9ImJ1dHQiPjwvcGF0aD48Zz48cGF0aCBkPSJNODYsMjEuNWMtMzUuNjIyMzcsMCAtNjQuNSwyOC44Nzc2MyAtNjQuNSw2NC41YzAsMzUuNjIyMzcgMjguODc3NjMsNjQuNSA2NC41LDY0LjVjMzUuNjIyMzcsMCA2NC41LC0yOC44Nzc2MyA2NC41LC02NC41YzAsLTM1LjYyMjM3IC0yOC44Nzc2MywtNjQuNSAtNjQuNSwtNjQuNXoiIGZpbGw9IiMyZWNjNzEiIHN0cm9rZT0ibm9uZSIgc3Ryb2tlLXdpZHRoPSIxIiBzdHJva2UtbGluZWNhcD0iYnV0dCI+PC9wYXRoPjxwYXRoIGQ9Ik04NiwzMy41OTM3NWMtMjguOTQzMTcsMCAtNTIuNDA2MjUsMjMuNDYzMDggLTUyLjQwNjI1LDUyLjQwNjI1YzAsMjguOTQzMTcgMjMuNDYzMDgsNTIuNDA2MjUgNTIuNDA2MjUsNTIuNDA2MjVjMjguOTQzMTcsMCA1Mi40MDYyNSwtMjMuNDYzMDggNTIuNDA2MjUsLTUyLjQwNjI1YzAsLTI4Ljk0MzE3IC0yMy40NjMwOCwtNTIuNDA2MjUgLTUyLjQwNjI1LC01Mi40MDYyNXoiIGZpbGw9IiNmZmZmZmYiIHN0cm9rZT0ibm9uZSIgc3Ryb2tlLXdpZHRoPSIxIiBzdHJva2UtbGluZWNhcD0iYnV0dCI+PC9wYXRoPjxwYXRoIGQ9Ik04NiwyMS41Yy0zNS42MjIzNywwIC02NC41LDI4Ljg3NzYzIC02NC41LDY0LjVjMCwzNS42MjIzNyAyOC44Nzc2Myw2NC41IDY0LjUsNjQuNWMzNS42MjIzNywwIDY0LjUsLTI4Ljg3NzYzIDY0LjUsLTY0LjVjMCwtMzUuNjIyMzcgLTI4Ljg3NzYzLC02NC41IC02NC41LC02NC41eiIgZmlsbD0ibm9uZSIgc3Ryb2tlPSIjZmZmZmZmIiBzdHJva2Utd2lkdGg9IjguMDYyNSIgc3Ryb2tlLWxpbmVjYXA9ImJ1dHQiPjwvcGF0aD48cGF0aCBkPSJNNTYuNDM3NSw5Mi43MTg3NWwxOC4yMDc4MSwxNi4xMjVsNDAuOTE3MTksLTQ3LjAzMTI1IiBmaWxsPSJub25lIiBzdHJva2U9IiMyZWNjNzEiIHN0cm9rZS13aWR0aD0iOC4wNjI1IiBzdHJva2UtbGluZWNhcD0icm91bmQiPjwvcGF0aD48L2c+PC9nPjwvc3ZnPg=='; }

  return 'https://img.icons8.com/cotton/64/000000/checkmark.png';
};

const ButtonWrapper = styled.div`
  display: flex;
  justify-content: space-between;
  flex-direction: row-reverse;
  margin: 1vh 1vw;
`;

const Button = styled.button`
  min-width: 80px;
  padding: 1vh 1vw;
  font-size: 16px;
  align-self: center;
`;

const TextWrapper = styled.div`
  display: flex;
  flex-direction: row;
  align-items: center;
  margin-bottom: 1vh;

  *:hover {
    cursor: pointer;
  }
`;

const InlineTextWrapper = styled.div`
  display: flex;
  flex-direction: row;
  align-items: center;

  > * {
    margin: 0px;
    padding: 0px 1vw 0px 0px;
  }

  > :first-child {
    padding-left: 1vw;
  }


  *:hover {
    cursor: pointer;
  }
`;

const GoogleMarker = styled.span`
  background-image: url(${props => getMarkerImage(props.color)});
  align-self: center;
  width: ${props => props.size};
  height: ${props => props.size};
  background-repeat: no-repeat;
  background-size: contain;
  display: inline-block;
`;

const getMarkerImage = (color) => {
  if(color == 'blue') { return 'http://maps.google.com/mapfiles/ms/icons/blue-dot.png'; }
  if(color == 'green') { return 'http://maps.google.com/mapfiles/ms/icons/green-dot.png'; }
  if(color == 'yellow') { return 'http://maps.google.com/mapfiles/ms/icons/yellow-dot.png'; }
  return 'http://maps.google.com/mapfiles/ms/icons/red-dot.png';
};

const Caret = styled.span`
  background-image: url(${props => getCaretImage(props.expand)});
  width: 20px;
  height: 20px;
  background-repeat: no-repeat;
  background-size: contain;
  display: inline-block;
  margin-right: 8px;
`;

const getCaretImage = (expand) => {
  if(expand) { return 'https://img.icons8.com/flat_round/64/000000/expand-arrow--v2.png'; }
  return 'https://img.icons8.com/flat_round/64/000000/collapse-arrow--v2.png'
}

export {
  Wrapper,
  Header,
  Footer,
  Status,
  FieldWrapper,
  Label,
  ReadOnly,
  Input,
  EmailValidationIcon,
  PasswordStrengthIcon,
  ButtonWrapper,
  Button,
  TextWrapper,
  InlineTextWrapper,
  GoogleMarker,
  Caret
};