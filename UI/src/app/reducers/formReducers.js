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



// ------------ mode manager reducer ------------------

import {
    OPEN_MODE_MANAGER_FORM,
    CLOSE_MODE_MANAGER_FORM,
    UPDATE_MODES
} from '../actions/formActions'

const modeFormInit = {
    visible : false,
    modes: []
}

import { 
    getArrayIndexByValue, 
    getShortNameFixtureMode,
    FixtureMode
} from '../util/util'
import {
    MODE_COLUMN_VALUES,
    MODE_ROW_VALUES    
} from '../constants/const'

import store from '../store'
export const modeManagerFormReducer = (state = modeFormInit, action) => {
    switch(action.type) {
        case OPEN_MODE_MANAGER_FORM: {
            return { ...state, visible:true }
        }
        case CLOSE_MODE_MANAGER_FORM: {
            return { ...state, visible: false }
        }
        case OPEN_FIX_FORM_NEW: {
            return { ...state, modes: []}
        }
        case OPEN_EDIT_FIXTURE_MODE_FORM: {                
            return { ... state, modes: getSelectedFixtureModeSet()}
        }
        case UPDATE_MODES: {
            return { ...state, modes: action.payload}
        }
    }
    return state
}

const getModeIdentifyer = (mode) => {
    return ' ' + mode.typeIndex + mode.colIndex + mode.rowIndex
}

const getSelectedFixtureModeSet = () => {
    
    // get selected fixture ids
    const selected = store.getState().selectionReducer.fixtures

    // get all fixture objects
    const fixtures = []
    const state = store.getState().stateReducer.data
    const outFixtures = state.Outputs.forEach(out => {
        out.Fixtures.forEach(fix => {fixtures.push(fix)})
    })
    state.FixturePool.forEach(fix => {fixtures.push(fix)})

    // get all fixture modes
    const modes = [] 
    fixtures.forEach(fix => {
        selected.forEach(id => {
            if(fix.Id == id) {
                fix.Modes.forEach( mode => {
                    modes.push(
                        {
                            typeIndex: getArrayIndexByValue(FixtureMode.all(), mode.Name),
                            colIndex: getArrayIndexByValue(MODE_COLUMN_VALUES, mode.Params[0]),
                            rowIndex: getArrayIndexByValue(MODE_ROW_VALUES, mode.Params[1])
                        }
                    )
                })
            }
        });
    })

    // create set of modes
    const modeSet = []
    const modeIds = []
    modes.forEach(mode => {
        const id = getModeIdentifyer(mode)
        if(getArrayIndexByValue(modeIds, id) == null) {
            modeIds.push(id)
            modeSet.push(mode)
        }
    })
    return modeSet
}

import { SUBMIT_MODES } from '../actions/formActions'
import { STATE_CHANGE_SERVER_ERROR } from '../actions/stateActions';

export const modesReducer = (state = { modes: [] }, action) => {
    switch(action.type) {
        case SUBMIT_MODES: {
            return {...state, modes: action.payload }
        }
        case OPEN_FIX_FORM_NEW: {
            return { ...state, modes: []}
        }
        case OPEN_EDIT_FIXTURE_MODE_FORM: {                
            return { ... state, modes: getSelectedFixtureModeSet()}
        }
    }
    return state
}

// ------------ error reducer ---------------------

import {CLOSE_ERROR_FORM, CONNECTION_ERROR} from '../actions/formActions'

const errorState = {
    visible: false,
    msg: null,
}

export const errorReducer = (state = errorState, action) => {
    switch(action.type) {
        case STATE_CHANGE_SERVER_ERROR: {
            let data = getErrorData(action.payload)
            return {...state, 
                visible: data.isVisible, 
                msg: data.msg }
        }
        case CONNECTION_ERROR: {
            return {...state,
                visible: true,
                msg: "Connection error " + action.payload
            }
        }
        case CLOSE_ERROR_FORM: {
            return {...state,
                visible: false,
                msg: null
            }
        }
    }
    return state
}

const getErrorData = (error) => {
    let isVisible = error != null ? true : false
    let msg = error != null ? 
        error.Content : "Unknow error accured"
    return {
        isVisible: isVisible,
        msg: msg
    }
}

