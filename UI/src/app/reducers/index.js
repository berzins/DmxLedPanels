import {combineReducers} from 'redux';
import {
    stateReducer,
    savedStatesReducer
} from './stateReducer'
import {
    outputFormReducer, 
    fixtureFormReducer, 
    formErrorReducer,
    storeStateFormReducer,
    loadStateFormReducer,
    fileSelectedReducer
} from './formReducers'
import {selectionReducer, poolItemSelectedReducer} from './selectionReducer'

const reducers = combineReducers(
    {
        stateReducer: stateReducer,
        outputFormReducer: outputFormReducer,
        fixtureFormReducer: fixtureFormReducer,
        formErrorReducer: formErrorReducer,
        selectionReducer: selectionReducer,
        storeStateFormReducer: storeStateFormReducer,
        fileSelectedReducer: fileSelectedReducer,
        loadStateFormReducer: loadStateFormReducer,
        savedStatesReducer: savedStatesReducer        
    }
);

export default reducers;