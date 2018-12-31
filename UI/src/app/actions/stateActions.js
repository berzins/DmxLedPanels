import axios from 'axios'
import { API_URL } from '../constants/const'
import { getCookie } from '../util/util';


export const STATE_CHANGE_SUCCESS = "STATE_CHANGE_SUCCESS"
export const STATE_CHANGE_SERVER_ERROR = "STATE_CHANGE_ERROR"
export const CONNECTION_ERROR = "CONNECTION_ERROR"
export const LOAD_STATE = "LOAD_STATE"


// evnets --> additional events to dispatch
export const requestServer = (url, dispatch, events = null) => {
    console.log("URL = " + url)
    axios.get(url)
    .then((response) => {
        if(response.status == 200) {
            if(events !== null) {
                events.forEach(e => {
                    dispatch({type: e, payload: response.data})
                });
            }
        } else {
            console.log(response.data)
            dispatch({type: STATE_CHANGE_SERVER_ERROR, payload: response.data})
        }
    })
    .catch((error) => {
        console.log(response.data)
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
    '&ip=' + data.ip.replace(/\./g, "_") +
    '&increment=' + data.increment 

    return (dispatch) => {
        requestServer(url, dispatch, [STATE_CHANGE_SUCCESS])
    }
}

export const editOutput = (ids, data) => {
    let url = API_URL + '/editOutput/?' + 
    'output_id=' + ids.join('|') + 
    '&name=' + data.name + 
    '&port=' + data.net + '|' + data.sub + '|' + data.uni +
    '&increment=' + data.increment 

    return (dispatch) => {
        requestServer(url, dispatch, [STATE_CHANGE_SUCCESS])
    }
}

export const editOutputName = (ids, data) => {
    let url = API_URL + '/editOutputName/?' + 
    'output_id=' + ids.join('|') + 
    '&name=' + data.name 

    return (dispatch) => {
        requestServer(url, dispatch, [STATE_CHANGE_SUCCESS])
    }
}

export const editOutputPort = (ids, data) => {
    let url = API_URL + '/editOutputPort/?' + 
    'output_id=' + ids.join('|') + 
    '&port=' + data.net + '|' + data.sub + '|' + data.uni

    return (dispatch) => {
        requestServer(url, dispatch, [STATE_CHANGE_SUCCESS])
    }
}

export const editOutputIp = (ids, data) => {
    let url = API_URL + '/editOuotputIP/?' + 
    'output_id=' + ids.join('|') + 
    '&ip=' + data.ip 

    return (dispatch) => {
        requestServer(url, dispatch, [STATE_CHANGE_SUCCESS])
    }
}


const createModeString = modes => {
    return modes.map(m => {return m.typeValue + '_' + m.colValue + '|' + m.rowValue}).join('^')
}

export const addFixture = (count, data) => {
 

    let url = API_URL + '/createFixture/?' + 
    'count=' + count + 
    '&name=' + data.name + 
    '&port=' + data.net + '|' + data.sub + '|' + data.uni +
    '&address=' + data.addr +
    '&utils_enabled=' + 'true' + // TODO: put radio switch in create fixture form
    '&utils_address=' + data.utilAddr +
    '&increment=' + data.increment +
    '&patch_type=' + data.patch + '_' + data.patchCol + '|' + data.patchRow +
    '&modes=' + createModeString(data.modes)

    return (dispatch) => {
        requestServer(url, dispatch, [STATE_CHANGE_SUCCESS])
    }
}

export const editFixture = (ids, data) => {
    
    let url = API_URL + '/editFixture/?' + 
    'fixture_id=' + ids.join('|') + 
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

export const editFixtureName = (ids, data) => {
    let url = API_URL + '/editFixtureName/?' + 
    'fixture_id=' + ids.join('|') + 
    '&name=' + data.name
    return (dispatch) => {
        requestServer(url, dispatch, [STATE_CHANGE_SUCCESS])
    }
}

export const editFixtureAddress = (ids, data) => {

    let url = API_URL + '/editFixtureAddress/?' + 
    'fixture_id=' + ids.join('|') + 
    '&port=' + data.net + '|' + data.sub + '|' + data.uni +
    '&address=' + data.addr +
    '&util_address=' + data.utilAddr +
    '&util_enabled=' + 'true' +
    '&increment=' + data.increment 

    return (dispatch) => {
        requestServer(url, dispatch, [STATE_CHANGE_SUCCESS])
    }
}

export const editFixtureMode = (ids, data) => {
    let url = API_URL + '/editFixtureMode/?' + 
    'fixture_id=' + ids.join('|') + 
    '&modes=' + createModeString(data.modes) 

    return (dispatch) => {
        requestServer(url, dispatch, [STATE_CHANGE_SUCCESS])
    }
}

export const editFixturePatch = (ids, data) => {
    let url = API_URL + '/editFixturePixelPatch/?' + 
    'fixture_id=' + ids.join('|') + 
    '&patch_type=' + data.patch + '_' + data.patchCol + '|' + data.patchRow

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


export const HIGHLIGHT_STATE_UPDATED = "HIGHLIGHT_STATE_UPDATED"

export const getHighlightState = () => {
    const url = API_URL + '/getHighlightState/'
    return(dispatch) => {
        requestServer(url, dispatch, [HIGHLIGHT_STATE_UPDATED])
    }
}

export const enableHighlight = (on) => {
    const url = API_URL + '/enableHighlight/?enabled=' + 
    (on ? "true" : "false")
    return(dispatch) => {
        requestServer(url, dispatch, [HIGHLIGHT_STATE_UPDATED])
    }
}

export const undoState = () => {
    const url = API_URL + '/undoState/'
    return (dispatch) => {
        requestServer(url, dispatch, [STATE_CHANGE_SUCCESS])
    }
}

export const redoState = () => {
    const url = API_URL + '/redoState/'
    return (dispatch) => {
        requestServer(url, dispatch, [STATE_CHANGE_SUCCESS])
    }
}


export const DMX_STATE_UPDATE = "DMX_STATE_UPDATE" 

export const getUpdateDmxStateDispatchObject = (hasSignal) => {
    return {
        type: DMX_STATE_UPDATE,
        payload: hasSignal
    }
}

export const PORT_DMX_STATE_UPDATE = "PORT_DMX_STATE_UPDATE"

export const getUpdatePortDmxStateDispatchObject = (activePorts) => {
    return {
        type: PORT_DMX_STATE_UPDATE,
        payload: activePorts
    }
}

export const CURRENT_PROJECT_UPDATE = "CURRENT_PROJECT_UPDATE"

export const getCurretProject = () => {
    const url = API_URL + '/currentProject/'
    return (dispatch) => {
        requestServer(url, dispatch, [CURRENT_PROJECT_UPDATE])
    }
}

export const LOGGIN_ACTION = "LOGGIN_ACTION"

export const loggIn = (password) => {
    const url = API_URL + '/session/?' +
    'pass=' + password
    return (dispatch) => {
        requestServer(url, dispatch, [LOGGIN_ACTION])
    }
}

// ------------- fixture template stuff ----------------------

export const FIXTURE_TEMPLATE_ACTION = "FIXTURE_TEMPLATE_ACTION"

export const storeFixtureTemplate = (fixtureId) => {
    const url = API_URL + "/storeFixtureTemplate/?" +
    'fixture_id=' + fixtureId
    return (dispatch) => {
        requestServer(url, dispatch, [FIXTURE_TEMPLATE_ACTION])
    }
}

export const getFixtureTemplates = () => {
    const url = API_URL + "/getTemplates/?"
    return (dispatch) => {
        requestServer(url, dispatch, [FIXTURE_TEMPLATE_ACTION])
    }
}



// ------------- end of fixture template stuff ----------------



