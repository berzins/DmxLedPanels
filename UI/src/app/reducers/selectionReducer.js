
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


const printSomething = (state) => {
    console.log(state.fixtures)
    console.log(state.outputs)
    console.log(
        ", has fix = " + state.hasFixture +
        ", has out = " + state.hasOutput +
        ", only fix = " + state.onlyFixture + 
        ", only out = "+  state.onlyOutput
    )
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
    let fix = state.fixtures.filter(fixId => { if(fixId !== id) {return fixId} })
    let hasFix = fix.length > 0 ? true : false
    const onlyFix = hasFix && !state.hasOutput
    return {
        ...state,
        fixtures: fix,
        hasFixture: hasFix,
        onlyFixture: onlyFix,
        onlyOutput: !onlyFix
    }
}

const deselectOutput = (state, id) => {
    let out = state.outputs.filter(outId => {if(outId  !== id) { return outId} })
    let hasOut = out.length > 0 ? true : false
    const onlyOut = hasOut && !state.hasFixture
    return {
        ...state,
        outputs: out,
        hasOutput: hasOut,
        onlyOutput: onlyOut,
        onlyFixture:!onlyOut
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
            s
        }

        case DESELECT_ALL: {
            return {...init, fixtures: [], outputs: []}
        }
    }
    return state
}
