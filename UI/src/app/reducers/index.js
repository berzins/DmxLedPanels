import {combineReducers} from 'redux';
import {
    stateReducer,
    savedStatesReducer,
    hilightStateReducer
} from './stateReducer'
import {
    outputFormReducer, 
    fixtureFormReducer, 
    formErrorReducer,
    storeStateFormReducer,
    loadStateFormReducer,
    fileSelectedReducer,
    fixtureEditNameFormReducer,
    fixtureEditAddressFormReducer,
    fixtureEditModeFormReducer,
    fixtureEditPatchFormReducer
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
        savedStatesReducer: savedStatesReducer,
        hilightStateReducer: hilightStateReducer,
        fixtureEditNameFormReducer: fixtureEditNameFormReducer,
        fixtureEditAddressFormReducer: fixtureEditAddressFormReducer,
        fixtureEditModeFormReducer: fixtureEditModeFormReducer,
        fixtureEditPatchFormReducer: fixtureEditPatchFormReducer
    }
);

export default reducers;