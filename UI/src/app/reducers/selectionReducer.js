
import {
    SELECT_FIXTURE,
    SELECT_OUTPUT,
    DESELECT_FIXTURE,
    DESELECT_OUTPUT,
    DESELECT_ALL
} from '../actions/selectActions'

const init = {
    fixtures: [],
    outputs: [],
    hasFixture: false,
    hasOutput: false,
    onlyFixture: false,
    onlyOutput: false
}

const newSelection = {
    ...init,
    fixtures: [],
    outputs: []
}

const removeItem = (array, id) => {
    const index = array.indexOf(id)
    if(index > -1) { 
        array.splice(index, 1)
    }
}

const selectFixture = (state, id) => {
    if(state.hasOutput) {
        state = {...init, fixtures: [], outputs: [] }
    }
    state.fixtures.push(id)
    return {...state,
        onlyOutput: false,
        hasFixture: true,
        onlyFixture: !state.onlyOutput
    }
}

const selectOutput = (state, id) => {
    if(state.hasFixture) {
        state = {...init, fixtures: [], outputs: [] }
    }
    state.outputs.push(id)
    return {
        ...state,
        hasOutput: true,
        onlyFixture: false,
        onlyOutput: !state.onlyFixture
    }
}

const deselectFixture = (state, id) => {
    removeItem(state.fixtures, id)
    const fix = state.fixtures
    let hasFix = fix.length > 0 ? true : false
    const onlyFix = hasFix && !state.hasOutput
    return {
        ...state,
        fixtures: fix,
        hasFixture: hasFix,
        onlyFixture: onlyFix,
        onlyOutput: (!onlyFix && state.hasOutput)
    }
}

const deselectOutput = (state, id) => {
    removeItem(state.outputs, id)
    let out = state.outputs
    let hasOut = out.length > 0 ? true : false
    const onlyOut = hasOut && !state.hasFixture
    return {
        ...state,
        outputs: out,
        hasOutput: hasOut,
        onlyOutput: onlyOut,
        onlyFixture: (!onlyOut && state.hasFixture)
    }
}

export const selectionReducer = (state = init, action) => {
    switch(action.type) {
        case SELECT_FIXTURE: {
            let s = selectFixture(state, action.payload.id)
            return s
        } 

        case SELECT_OUTPUT: {            
            let s = selectOutput(state, action.payload.id)
            return s

        }

        case DESELECT_FIXTURE: {
            let s = deselectFixture(state, action.payload.id)
            return s
        }

        case DESELECT_OUTPUT: {
            let s = deselectOutput(state, action.payload.id)
            return s
        }

        case DESELECT_ALL: {
            return {...init, fixtures: [], outputs: []}
        }
    }
    return state
}
