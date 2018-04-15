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
    filename: null
}

export const fileSelectedReducer = (state = fileInit, action)  =>{
    switch(action.type) {
        case SELECT_FILE: {
            return {...state, filename: action.payload }
        }
    }
    return state
}



export const formErrorReducer = (state = null, actoin) => {
    switch(actoin.type) {
        case FORM_VALUE_ERROR: {
            return actoin.payload
        }
    }
    return state;
}
