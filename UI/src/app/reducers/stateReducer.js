import {
    STATE_CHANGE_SUCCESS,
    STATE_CHANGE_SERVER_ERROR,
    CONNECTION_ERROR,
    LOAD_STATE,
    SAVED_STATES_UPDATED,
    getHighlightState
} from '../actions/stateActions'

import {
    SOMETHING_HIGHLIGHTED
} from '../actions/actions'

const init = {
    data: null,
    error: null,
    loaded: false,
}

export const stateReducer = (state = init, action) => {
    console.log(action.type)
    switch(action.type) {
        case LOAD_STATE: {
            return{...sate, loaded:false, error: null, data:null }
        }
        case STATE_CHANGE_SUCCESS: {
            return {...state, 
                loaded:true,  
                error:null, 
                data: action.payload }
        }
        case STATE_CHANGE_SERVER_ERROR: {
            return {...state, 
                loaded:true, 
                error: action.payload }
        }
        case CONNECTION_ERROR: {
            return {...state, loaded: true,
                 error: { type: 'connection-error', content: action.payload } , 
                 data: null
            }
        }
    }
    return state;
}

const savedInit = {
    names: []
}

export const savedStatesReducer = (state = savedInit, action) => {
    switch(action.type) {
        case SAVED_STATES_UPDATED: {
            if(action.payload.Type == 'saved_states') {
                return {...state, names: action.payload.Content}
            }
        }
    }
    return state
}


import { HIGHLIGHT_STATE_UPDATED } from '../actions/stateActions'

const highlightInit = {
    on: false,
    fixtures: []
}

export const hilightStateReducer = (state = getHighlightState, action) => {
    switch(action.type) {
        case HIGHLIGHT_STATE_UPDATED: {
            return {...state, on: action.payload.Content}
        }
        case SOMETHING_HIGHLIGHTED: {
            return {...state, fixtures: action.payload.Content}
        }
    }
    return state
}

import { DMX_STATE_UPDATE } from '../actions/stateActions'

export const dmxStateReducer = (state = false, action) => {
    switch(action.type) {
        case DMX_STATE_UPDATE: {
            return action.payload.Content
        }
    }
    return state;
}

import { CURRENT_PROJECT_UPDATE } from'../actions/stateActions'

export const currentProjectReducer = (state = {name : null}, action) => {
    switch(action.type) {
        case CURRENT_PROJECT_UPDATE: {
            return {...state, name: action.payload.Content.split('.')[0]}
        }
    }
    return state
}



