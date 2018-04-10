import axios from 'axios'
import API_URL from '../constants/const'


export const STATE_CHANGE_SUCCESS = "STATE_CHANGE_SUCCESS"
export const STATE_CHANGE_SERVER_ERROR = "STATE_CHANGE_ERROR"
export const CONNECTION_ERROR = "CONNECTION_ERROR"
export const LOAD_STATE = "LOAD_STATE"


const requestServer = (url, dispatch) => {
    axios.get(url)
    .then((response) => {
        if(response.status == 200) {
            dispatch({type: STATE_CHANGE_SUCCESS, payload: response.data})
        } else {
            dispatch({type: STATE_CHANGE_SERVER_ERROR, payload: response.data})
        }
    })
    .catch((error) => {
        dispatch({type: CONNECTION_ERROR, payload: error})
    })
}

export const loadState = () => {
    let url = API_URL + "/getState/"
    return (dispatch) => {
        requestServer(url, dispatch)
    }
}

export const addOutput = (count, name, port, increment) => {
    let net = port[0]
    let sub = port[1]
    let uni = port[2]
    let url = API_URL + '/createOutput/?count=${count}&name=${name}&port=${net}|${sub}|${uni}&increment=${increment}'
    return (dispatch) => {
        requestServer(url, dispatch)
    }
}



