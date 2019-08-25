import styled from 'styled-components';

import icons from '@src/icons';

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
  padding: 1vh 1vw;
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

  > button {
    margin: 1vh 1vw 1vh 0px;
  }
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
    return icons.FILLED_CHECK_MARK;
  }

  return icons.CROSS_MARK;
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
  if(value.length < 6) { return icons.CROSS_MARK; }

  let strength = 0;
  if(/[A-Z]/.test(value)) { strength++; }
  if(/[a-z]/.test(value)) { strength++; }
  if(/[0-9]/.test(value)) { strength++; }
  if(/[^A-Za-z0-9]/.test(value)) { strength++; }
  if(value.length >= 8) { strength++; }

  if(strength <= 3) { return icons.CHECK_MARK; }

  return icons.FILLED_CHECK_MARK;
};

const ButtonWrapper = styled.div`
  display: flex;
  justify-content: space-between;
  flex-direction: row-reverse;
  margin: 1vh 1vw;

  > span:last-child {
    padding: 1vh 1vw 1vh 0px;
    margin: 1vh 1vw 1vh 0px;
  }
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
  if(color == 'blue') { return icons.BLUE_DOT; }
  if(color == 'green') { return icons.GREEN_DOT; }
  if(color == 'yellow') { return icons.YELLOW_DOT; }
  return icons.RED_DOT;
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
  if(expand) { return icons.EXPAND; }
  return icons.COLLAPSE;
}

const Arrow = styled.span`
  background-image: url(${props => getArrowImage(props.direction)});
  align-self: center;
  width: ${props => props.size};
  height: ${props => props.size};
  background-repeat: no-repeat;
  background-size: contain;
  display: inline-block;
  margin-top: 3px;
  margin-bottom: -3px;
`;

const getArrowImage = (direction) => {
  if(direction=='left') { return icons.LEFT; }
  return icons.RIGHT;
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
  Caret,
  Arrow
};