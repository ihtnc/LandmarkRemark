import styled from 'styled-components';

const RemarkWrapper = styled.div`
  font-size: 1.75vmax;
  display: flex;
  flex-direction: column;
  align-items: stretch;
  min-width: 200px;
`;

const RemarkHeader = styled.div`
  font-weight: bold;
  padding: 1vh 1vw;
`;

const RemarkStatus = styled.span`
  padding: 1vh 1vw;
  margin: 1vh 1vw;
  font-size: smaller;
  color: ${props => getStatusColor(props)};
  align-self: flex-start;
  display: ${props => getStatusDisplay(props)};
`;

const getStatusColor = (props) => {
  if(props.error) { return 'red'; }
  return 'gray';
};

const getStatusDisplay = (props) => {
  if(props.show) { return 'flex'; }
  return 'none';
};

const RemarkLabel = styled.div`
  font-size: larger;
  padding: 1vh 1vw;
`;

const RemarkInput = styled.input`
  font-size: larger;
  padding: 1vh 1vw;
  margin: 1vh 1vw;
  border: 1px solid;
`;

const RemarkButtonWrapper = styled.div`
  display: flex;
  justify-content: flex-end;
`;

const RemarkButton = styled.button`
  min-width: 80px;
  padding: 1vh 1vw;
  margin: 1vh 1vw 1vh 0;
  font-size: 1.5vmax;
  align-self: center;
`;

export {
  RemarkWrapper,
  RemarkHeader,
  RemarkStatus,
  RemarkLabel,
  RemarkInput,
  RemarkButtonWrapper,
  RemarkButton
};