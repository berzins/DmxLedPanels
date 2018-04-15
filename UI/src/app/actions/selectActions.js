
import { selection } from '../state/selection'


export const SELECT_ITEM = "SELECT_ITEM"
export const SELECT_FIXTURE = "SELECT_FIXTURE" 
export const SELECT_OUTPUT = "SELECT_OUTPUT"

export const DESELECT_ITEM = "DESELECT_ITEM"
export const DESELECT_FIXTURE = "DESELECT_FIXTURE" 
export const DESELECT_OUTPUT = "DESELECT_OUTPUT"

export const DESELECT_ALL = "DESELECT_ALL"

export const SELECT_TYPE_FIXTURE = "SELECT_TYPE_FIXTURE"
export const SELECT_TYPE_OUTPUT = "SELECT_TYPE_FIXTURE"




export const selectFixture = (id, single) => {
    return (dispatch) => {
        let p = {type:SELECT_TYPE_FIXTURE, id: id, single: single}
        dispatch({type:SELECT_FIXTURE, payload: p})
        dispatch({type:SELECT_ITEM, payload: p})
    }
}

export const deselectFixture = (id) => {
    return (dispatch) => {        
        let p = {type:SELECT_TYPE_FIXTURE, id: id}
        dispatch({type:DESELECT_FIXTURE, payload: p})
        dispatch({type:DESELECT_ITEM, payload: p})
    }
}

export const selectOutput = (id, single) => {
    return (dispatch) => {
        let p = {type:SELECT_TYPE_OUTPUT, id: id, single: single}
        dispatch({type: SELECT_OUTPUT, payload: p})
        dispatch({type: SELECT_ITEM, payload: p})
    }
}

export const deselectOutput = (id) => {
    return (dispatch) => {
        let p = {type:SELECT_TYPE_OUTPUT, id: id}
        dispatch({type: DESELECT_OUTPUT, payload: p})
        dispatch({type: DESELECT_ITEM, payload: p})
    }
}

export const deselectAll = () => {
    return (dispatch) => {
        dispatch({type: DESELECT_ALL})
    }
}
