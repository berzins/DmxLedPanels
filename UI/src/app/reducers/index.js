import {combineReducers} from 'redux';
import {
    stateReducer,
    savedStatesReducer,
    hilightStateReducer,
    dmxStateReducer
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
    fixtureEditPatchFormReducer,
    outputEditNameFormReducer,
    outputEditPortFormReducer,
    outputEditIpFormReducer,
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
        fixtureEditPatchFormReducer: fixtureEditPatchFormReducer,
        outputEditNameFormReducer: outputEditNameFormReducer,
        outputEditPortFormReducer: outputEditPortFormReducer,
        outputEditIpFormReducer: outputEditIpFormReducer,
        dmxStateReducer: dmxStateReducer
    }
);

export default reducers;