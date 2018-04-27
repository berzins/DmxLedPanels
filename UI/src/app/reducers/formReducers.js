import {
    OPEN_OUT_FORM_NEW,
    OPEN_OUT_FORM_EDIT,
    OPEN_FIX_FORM_NEW,
    OPEN_FIX_FORM_EDIT,
    CLOSE_OUT_FORM,
    CLOSE_FIX_FORM,
    OPEN_STORE_STATE_FORM,
    CLOSE_SOTRE_STATE_FORM,
    OPEN_LOAD_STATE_FORM,
    CLOSE_LOAD_STATE_FORM,
    MODE_NEW,
    MODE_EDIT,
    MODE_DEFAULT,

    FORM_VALUE_ERROR,
    SELECT_FILE,
    FormMode
} from '../actions/formActions'


const init = {
    opened: false,
    mode: null,
    data: null
}

export const outputFormReducer = (state = init, action) => {
    switch(action.type) {
        case OPEN_OUT_FORM_NEW: {
            return {...state, opened: true, mode: MODE_NEW, data: action.payload }
        }
        case OPEN_OUT_FORM_EDIT: {
            return {...state, opened: true, mode: MODE_EDIT, data: action.payload }
        }
        case CLOSE_OUT_FORM: {
            return {...state, opened: false, mode: null, data: null }
        }
    }
    return state
}



export const fixtureFormReducer = (state = init, action) => {
    switch(action.type) {
        case OPEN_FIX_FORM_NEW: {
            return {...state, opened: true, mode: MODE_NEW, data: action.payload }
        }
        case OPEN_FIX_FORM_EDIT: {
            return {...state, opened: true, mode: MODE_EDIT, data: action.payload }
        }
        case CLOSE_FIX_FORM: {
            return {...state, opened: false, mode: null, data: null }
        }
    }
    return state
}


export const storeStateFormReducer = (state = init, action) => {
    switch(action.type) {
        case OPEN_STORE_STATE_FORM: {
            return {...state, opened: true, mode: MODE_DEFAULT, data: action.payload }
        }
        case CLOSE_SOTRE_STATE_FORM: {
            return {...state, opened: false, mode: null, data: action.payload}
        }
    }
    return state
}


export const loadStateFormReducer = (state = init, action) => {
    switch(action.type) {
        case OPEN_LOAD_STATE_FORM: {
            return {...state, opened: true, mode: MODE_DEFAULT, data: action.payload }
        }
        case CLOSE_LOAD_STATE_FORM: {
            return {...state, opened: false, mode: null, data: action.payload}
        }
    }
    return state
}


const  fileInit = {
    filename: null,
    selector : null
}

export const fileSelectedReducer = (state = fileInit, action)  =>{
    switch(action.type) {
        case SELECT_FILE: {
            return {
                ...state, 
                filename: action.payload.filename,
                selector: action.payload.selector      
            }
        }
    }
    return state
}


import { CLEAR_VALUE_ERROR } from '../actions/formActions'

export const formErrorReducer = (state = null, actoin) => {
    switch(actoin.type) {
        case FORM_VALUE_ERROR: {
            return actoin.payload
        }
        case CLEAR_VALUE_ERROR: {
            return null
        }
    }
    return state;
}



// --------------- edit reducers -----------------------


import { 
    OPEN_EDIT_FIXTURE_NAME_FORM,
    CLOSE_EDIT_FIXTURE_NAME_FORM
} from '../actions/formActions'

export const fixtureEditNameFormReducer = (state = init, action) => {
    return genericEditFormEventHandler(
        state, action,
        OPEN_EDIT_FIXTURE_NAME_FORM,
        CLOSE_EDIT_FIXTURE_NAME_FORM
    )
}



import { 
    OPEN_EDIT_FIXTURE_ADDRESS_FORM,
    CLOSE_EDIT_FIXTURE_ADDRESS_FORM
} from '../actions/formActions'

export const fixtureEditAddressFormReducer = (state = init, action) => {
    return genericEditFormEventHandler(
        state, action,
        OPEN_EDIT_FIXTURE_ADDRESS_FORM,
        CLOSE_EDIT_FIXTURE_ADDRESS_FORM
    )
}



import { 
    OPEN_EDIT_FIXTURE_MODE_FORM,
    CLOSE_EDIT_FIXTURE_MODE_FORM
} from '../actions/formActions'

export const fixtureEditModeFormReducer = (state = init, action) => {
    return genericEditFormEventHandler(
        state, action,
        OPEN_EDIT_FIXTURE_MODE_FORM,
        CLOSE_EDIT_FIXTURE_MODE_FORM
    )
}



import { 
    OPEN_EDIT_FIXTURE_PATCH_FORM,
    CLOSE_EDIT_FIXTURE_PATCH_FORM
    
} from '../actions/formActions'

export const fixtureEditPatchFormReducer = (state = init, action) => {
    return genericEditFormEventHandler(
        state, action,
        OPEN_EDIT_FIXTURE_PATCH_FORM,
        CLOSE_EDIT_FIXTURE_PATCH_FORM
    )
}

import { 
    OPEN_EDIT_OUTPUT_NAME_FORM,
    CLOSE_EDIT_OUTPUT_NAME_FORM
    
} from '../actions/formActions'

export const outputEditNameFormReducer = (state = init, action) => {
    return genericEditFormEventHandler(
        state, action,
        OPEN_EDIT_OUTPUT_NAME_FORM,
        CLOSE_EDIT_OUTPUT_NAME_FORM
    )
}


import { 
    OPEN_EDIT_OUTPUT_PORT_FORM,
    CLOSE_EDIT_OUTPUT_PORT_FORM
} from '../actions/formActions'

export const outputEditPortFormReducer = (state = init, action) => {
    return genericEditFormEventHandler(
        state, action,
        OPEN_EDIT_OUTPUT_PORT_FORM,
        CLOSE_EDIT_OUTPUT_PORT_FORM
    )
}

import {
    OPEN_EDIT_OUTPUT_IP_FORM,
    CLOSE_EDIT_OUTPUT_IP_FORM
} from '../actions/formActions'

export const outputEditIpFormReducer = (state = init, action) => {
    return genericEditFormEventHandler(
        state, action,
        OPEN_EDIT_OUTPUT_IP_FORM,
        CLOSE_EDIT_OUTPUT_IP_FORM
    )
}


const genericEditFormEventHandler = (state, action, open, close) => {
    switch(action.type) {
        case open: {
            return {...state, opened: true, mode: MODE_DEFAULT, data: action.payload }
        }
        case close: {
            return {...state, opened: false, mode: null, data: action.payload}
        }
    }
    return state
}
