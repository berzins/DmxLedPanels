import axios from 'axios'
import { API_URL } from '../constants/const'


export const STATE_CHANGE_SUCCESS = "STATE_CHANGE_SUCCESS"
export const STATE_CHANGE_SERVER_ERROR = "STATE_CHANGE_ERROR"
export const CONNECTION_ERROR = "CONNECTION_ERROR"
export const LOAD_STATE = "LOAD_STATE"


// evnets --> additional events to dispatch
const requestServer = (url, dispatch, events = null) => {
    console.log("URL = " + url)
    axios.get(url)
    .then((response) => {
        if(response.status == 200) {
            if(events !== null) {
                events.forEach(e => {
                    dispatch({type: e, payload: response.data})
                    console.log(e + " dispatched")
                });
            }
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
        requestServer(url, dispatch, [STATE_CHANGE_SUCCESS])
    }
}

export const loadStateFromFile = (file) => {
    let url = API_URL + "/loadState/?file_name=" + file + ".json"
    return (dispatch) => {
        requestServer(url, dispatch, [STATE_CHANGE_SUCCESS])
    }
}


export const addOutput = (count, data) => {
    let url = API_URL + '/createOutput/?' + 
    'count=' + count + 
    '&name=' + data.name + 
    '&port=' + data.net + '|' + data.sub + '|' + data.uni +
    '&increment=' + data.increment 

    return (dispatch) => {
        requestServer(url, dispatch, [STATE_CHANGE_SUCCESS])
    }
}

export const addFixture = (count, data) => {
    
    let url = API_URL + '/createFixture/?' + 
    'count=' + count + 
    '&name=' + data.name + 
    '&port=' + data.net + '|' + data.sub + '|' + data.uni +
    '&address=' + data.addr +
    '&increment=' + data.increment +
    '&patch_type=' + data.patch + '_' + data.patchCol + '|' + data.patchRow +
    '&mode=' + data.mode + '_' + data.modeCol + '|' + data.modeRow

    return (dispatch) => {
        requestServer(url, dispatch, [STATE_CHANGE_SUCCESS])
    }
}

export const patchFixture = (fixIds, outId) => {

    let fids = fixIds.join('|')
    let url = API_URL + '/moveFixtureToOutput/?' + 
    'fixture_id=' + fids +
    '&output_id=' + outId
    return (dispatch) => {
        requestServer(url, dispatch, [STATE_CHANGE_SUCCESS])
    }
}

export const unpatchFixture = (fixIds) => {

    let fids = fixIds.join('|')
    let url = API_URL + '/moveFixtureToFixturePool/?' + 
    'fixture_id=' + fids
    return (dispatch) => {
        requestServer(url, dispatch, [STATE_CHANGE_SUCCESS])
    }
}

export const saveState = (name) => {
    let url = API_URL + '/saveState/?' + 
    'file_name=' + name

    return (dispatch) => {
        requestServer(url, dispatch)
    }
}

export const deleteOutput = ids => {
    let oids = ids.join('|')
    let url = API_URL + '/deleteOutput/?' + 
    'id=' + oids 

    return (dispatch) => {
        requestServer(url, dispatch, [STATE_CHANGE_SUCCESS])
    }
}

export const deleteFixture = ids => {
    let fids = ids.join('|')
    let url = API_URL + '/deleteFixture/?' + 
    'id=' + fids 

    return (dispatch) => {
        requestServer(url, dispatch, [STATE_CHANGE_SUCCESS])
    }
}


export const SAVED_STATES_UPDATED = "SAVED_STATES_UPDATED"

export const getSavedStates = () => {
    const url = API_URL + '/getSavedStates/'
    return(dispatch) => {
        requestServer(url, dispatch, [SAVED_STATES_UPDATED])
    }
}





