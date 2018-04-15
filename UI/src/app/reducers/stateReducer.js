import {
    STATE_CHANGE_SUCCESS,
    STATE_CHANGE_SERVER_ERROR,
    CONNECTION_ERROR,
    LOAD_STATE,
    SAVED_STATES_UPDATED
} from '../actions/stateActions'

const init = {
    data: null,
    error: null,
    loaded: false,
}

export const stateReducer = (state = init, action) => {

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