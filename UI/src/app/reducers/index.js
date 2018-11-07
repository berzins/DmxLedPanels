import {combineReducers} from 'redux';
import {
    stateReducer,
    savedStatesReducer,
    hilightStateReducer,
    dmxStateReducer,
    currentProjectReducer,
    sessionReducer,
    fixtureTemplateReducer,
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
    modeManagerFormReducer,
    modesReducer,
    errorReducer
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
        dmxStateReducer: dmxStateReducer,
        currentProjectReducer: currentProjectReducer,
        modeManagerFormReducer: modeManagerFormReducer,
        modesReducer: modesReducer,
        errorReducer: errorReducer,
        sessionReducer: sessionReducer,
        fixtureTemplateReducer: fixtureTemplateReducer
    }
);

export default reducers;