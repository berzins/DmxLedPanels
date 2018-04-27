

export const MODE_NEW = "MODE_NEW"
export const MODE_EDIT = "MODE_EDIT"
export const MODE_DEFAULT = "MODE_DEFAULT"

export const OPEN_OUT_FORM_NEW = "OPEN_OUT_FORM_NEW"
export const OPEN_OUT_FORM_EDIT = "OPEN_OUT_FORM_EDIT"


export const openOutputForm = (mode = MODE_NEW, data = null) => {
    return (dispatch) => {
        switch(mode) {
            case MODE_NEW: {
                dispatch({type: OPEN_OUT_FORM_NEW, pyload: data })
                break;    
            }
            case MODE_EDIT: {
                dispatch({type: OPEN_OUT_FORM_EDIT, payload: data })
            }
        }
    }
}

export const OPEN_FIX_FORM_NEW = "OPEN_FIX_FORM_NEW"
export const OPEN_FIX_FORM_EDIT = "OPEN_FIX_FORM_EDIT"

export const openFixtureForm = (mode = MODE_NEW, data = null) => {
    return (dispatch) => {
        switch(mode) {
            case MODE_NEW: {
                return dispatch({type: OPEN_FIX_FORM_NEW, pyload: data })
                break;    
            }
            case MODE_EDIT: {
                return dispatch({type: OPEN_FIX_FORM_EDIT, payload: data })
            }
        }
    }
}


export const CLOSE_FIX_FORM = "CLOSE_FIX_FORM"

export const closeFixtureForm = (data = null) => {
    return (dispatch) => {
        dispatch({type: CLOSE_FIX_FORM, payload: data })
    }
}


export const CLOSE_OUT_FORM = "CLOSE_OUT_FORM"

export const closeOutputForm = (data = null) => {
    return (dispatch) => {
        dispatch({type: CLOSE_OUT_FORM, payload:data })
    }
}


export const OPEN_STORE_STATE_FORM = "OPEN_STORE_STATE_FORM"

export const openStoreStateForm = (data = null) => {
    return (dispatch) => {
        dispatch({type: OPEN_STORE_STATE_FORM, payload:data})
    }
}

export const CLOSE_SOTRE_STATE_FORM = "CLOSE_STORE_STATE_FORM"

export const closeStoreStateForm = (data = null) => {
    return(dispatch) => {
        dispatch({type: CLOSE_SOTRE_STATE_FORM, payload: data})
    }
}


export const OPEN_LOAD_STATE_FORM = "OPEN_LOAD_STATE_FORM"

export const openLoadStateForm = (data = null) => {
    return(dispatch) => {
        dispatch({type: OPEN_LOAD_STATE_FORM, payload: data})
    }
}


export const CLOSE_LOAD_STATE_FORM = "CLOASE_LOAD_STATE_FORM"

export const closeOpenStateForm = (data = null) => {
    return (dispatch) => {
        dispatch({type: CLOSE_LOAD_STATE_FORM, payload : data})
    }
}


export const FORM_VALUE_ERROR = "FORM_VALUE_ERROR"

export const riseFormValueError = (msg) => {
    return (dispatch) => {
        dispatch({type: FORM_VALUE_ERROR, payload:msg })
    }
}


export const CLEAR_VALUE_ERROR = "CLEAR_VALUE_ERROR"

export const clearFormValueError = (msg = null) => {
    return (dispatch) => {
        dispatch({type: CLEAR_VALUE_ERROR, payload:msg })
    }
}


export const SELECT_FILE = "SELECT_FILE"

export const selectFile = (fileItemId, selector = null) => { 
    return (dispatch) => {
        dispatch({type: SELECT_FILE, payload: {filename: fileItemId, selector: selector}})
    }
}


// ------------- edit form actions ----------------

export const OPEN_EDIT_FIXTURE_NAME_FORM = "OPEN_EDIT_FIXTURE_NAME_FORM"

export const openEditFixtureNameForm = (data = null) => {
    return dispatch => {
        dispatch({type: OPEN_EDIT_FIXTURE_NAME_FORM, payload:data})
    }
}


export const OPEN_EDIT_FIXTURE_ADDRESS_FORM = "OPEN_EDIT_FIXTURE_ADDRESS_FORM"

export const openEditFixtureAddressForm = (data = null) => {
    return dispatch => {
        dispatch({type: OPEN_EDIT_FIXTURE_ADDRESS_FORM, payload:data})
    }
}


export const OPEN_EDIT_FIXTURE_MODE_FORM = "OPEN_EDIT_FIXTURE_MODE_FORM"

export const openEditFixtureModeForm = (data = null) => {
    return dispatch => {
        dispatch({type: OPEN_EDIT_FIXTURE_MODE_FORM, payload:data})
    }
}


export const OPEN_EDIT_FIXTURE_PATCH_FORM = "OPEN_EDIT_FIXTURE_PATCH_FORM"

export const openEditFixturePatchForm = (data = null) => {
    return dispatch => {
        dispatch({type: OPEN_EDIT_FIXTURE_PATCH_FORM, payload:data})
    }
}


export const CLOSE_EDIT_FIXTURE_NAME_FORM = "CLOSE_EDIT_FIXTURE_NAME_FORM"

export const closeEditFixtureNameForm = (data = null) => {
    return dispatch => {
        dispatch({type: CLOSE_EDIT_FIXTURE_NAME_FORM, payload:data})
    }
}


export const CLOSE_EDIT_FIXTURE_ADDRESS_FORM = "CLOSE_EDIT_FIXTURE_ADDRESS_FORM"

export const closeEditFixtureAddressForm = (data = null) => {
    return dispatch => {
        dispatch({type: CLOSE_EDIT_FIXTURE_ADDRESS_FORM, payload:data})
    }
}


export const CLOSE_EDIT_FIXTURE_MODE_FORM = "CLOSE_EDIT_FIXTURE_MODE_FORM"

export const closeEditFixtureModeForm = (data = null) => {
    return dispatch => {
        dispatch({type: CLOSE_EDIT_FIXTURE_MODE_FORM, payload:data})
    }
}


export const CLOSE_EDIT_FIXTURE_PATCH_FORM = "CLOSE_EDIT_FIXTURE_PATCH_FORM"

export const closeEditFixturePatchForm = (data = null) => {
    return dispatch => {
        dispatch({type: CLOSE_EDIT_FIXTURE_PATCH_FORM, payload:data})
    }
}

export const OPEN_EDIT_OUTPUT_NAME_FORM = "OPEN_EDIT_OUTPUT_NAME_FORM"

export const openEditOutputNameForm = (data = null)  =>{
    return dispatch => {
        dispatch({type: OPEN_EDIT_OUTPUT_NAME_FORM, payload: data})
    }
}


export const OPEN_EDIT_OUTPUT_PORT_FORM = "OPEN_EDIT_OUTPUT_PORT_FORM"

export const openEditOutputPortForm = (data = null)  =>{
    return dispatch => {
        dispatch({type: OPEN_EDIT_OUTPUT_PORT_FORM, payload: data})
    }
}

export const OPEN_EDIT_OUTPUT_IP_FORM = "OPEN_EDIT_OUTPUT_IP_FORM"

export const openEditOutputIpForm = (data = null)  =>{
    return dispatch => {
        dispatch({type: OPEN_EDIT_OUTPUT_IP_FORM, payload: data})
    }
}


export const CLOSE_EDIT_OUTPUT_NAME_FORM = "CLOSE_EDIT_OUTPUT_NAME_FORM"

export const closeEditOutputNameForm = (data = null)  =>{
    return dispatch => {
        dispatch({type: CLOSE_EDIT_OUTPUT_NAME_FORM, payload: data})
    }
}


export const CLOSE_EDIT_OUTPUT_PORT_FORM = "CLOSE_EDIT_OUTPUT_PORT_FORM"

export const closeEditOutputPortForm = (data = null)  =>{
    return dispatch => {
        dispatch({type: CLOSE_EDIT_OUTPUT_PORT_FORM, payload: data})
    }
}

export const CLOSE_EDIT_OUTPUT_IP_FORM = "CLOSE_EDIT_OUTPUT_IP_FORM"

export const closeEditOutputIpForm = (data = null)  =>{
    return dispatch => {
        dispatch({type: CLOSE_EDIT_OUTPUT_IP_FORM, payload: data})
    }
}

// ------------- end of edit forms ----------------------------