import React, {Component} from 'react'
import {bindActionCreators} from 'redux'
import {connect} from 'react-redux'
import Modal from 'react-bootstrap4-modal' 
//import { fixtureFormReducer, formErrorReducer } from '../reducers/formReducers'
import { 
    closeFixtureForm, 
    riseFormValueError, 
    MODE_NEW, 
    MODE_EDIT, 
    clearFormValueError, 
    SELECT_FILE,
    openModeManagerForm
} from '../actions/formActions'
import { addFixture, editFixture } from '../actions/stateActions'
import { 
    FixtureMode, 
    FixturePatch, 
    isInteger, 
    fillIncrementArray,
    validateInteger,
    patchSizeValues
} from '../util/util'
import store from '../store'
import {
    formModal,
    rowItem,
    contentItem,
    textInputItem,
    selectItem,
    radioItem,
    buttonItem,
    errorItem
} from './editForms/formItems'
import ModeItemList from './justItems/modeItemList'
import { 
    MODE_TYPE_ID, 
    MODE_COLUMNS_ID, 
    MODE_ROWS_ID
 } from './justItems/modeItem'
import {convertModeIndexesToModevalues} from '../util/mode'
 

const FORM_ID_NAME = "FORM_ID_NAME"
const FOMR_ID_PATCH = "FOMR_ID_PATCH"
const FORM_ID_PATCH_COLS = "FORM_ID_PATCH_COLS"
const FORM_ID_PATCH_ROWS = "FORM_ID_PATCH_ROWS"
const FORM_ID_PORT_NET = "FORM_ID_PORT_NET"
const FORM_ID_PORT_SUB = "FORM_ID_PORT_SUB"
const FORM_ID_PORT_UNI = "FORM_ID_PORT_UNI"
const FORM_ID_PORT_ADR = "FORM_ID_PORT_ADR"
const FORM_IOD_PROT_UTIL_ADR = "FORM_IOD_PROT_UTIL_ADR"
const FORM_ID_ADR_INCREM = "FORM_ID_ADR_INCREM"
const FORM_ID_COUNT = "FORM_ID_COUNT"


class FixtureForm extends Component {

    constructor(props){
        super(props) 
        this.incrment = true
        this.modeItemList = null
    }

    handleClick() {
        this.props.click.clicked = true
    }

    fillIncrementArray(size, start) {
        let vals = Array.apply(null, Array(size))
        return vals.map((x, i) => {return i + start})
    }
    
    onClose(){
        this.props.closeFixtureForm(this)
    }

    validate(validatons) {
        let res = "";
        validatons.map((v) => { if(!v.valid) res = res + v.error + " ||| " })
        return res;
    }

    onSubmit() {

        let validatons = [];
        let ids = store.getState().selectionReducer.fixtures
        const count = this.getValue(FORM_ID_COUNT);
        
        console.log('increment value is')
        console.log(document.getElementById(FORM_ID_ADR_INCREM).checked)
        let data = {
            name : this.getValue(FORM_ID_NAME),
            patch : this.getValue(FOMR_ID_PATCH),
            patchCol : this.getValue(FORM_ID_PATCH_COLS),
            patchRow : this.getValue(FORM_ID_PATCH_ROWS),
            modes : convertModeIndexesToModevalues(store.getState().modesReducer.modes),
            net : this.getValue(FORM_ID_PORT_NET),
            sub : this.getValue(FORM_ID_PORT_SUB),
            uni : this.getValue(FORM_ID_PORT_UNI),
            addr : this.getValue(FORM_ID_PORT_ADR),
            utilAddr : this.getValue(FORM_IOD_PROT_UTIL_ADR),
            increment : document.getElementById(FORM_ID_ADR_INCREM).checked
        }
        
        validatons.push(validateInteger(1,512, "Address", data.addr))
        validatons.push(validateInteger(1, 512, "Util address", data.utilAddr))
        let res = this.validate(validatons)

        if(res !== "") {
            // we have form value error(s)
            this.props.riseFormValueError(res)
            return;
        }

        this.props.addFixture(count, data)    
        this.props.clearFormValueError()
        this.props.closeFixtureForm(null)
    }



    getValue(id){
        return document.getElementById(id).value
    }

    render() {

        let form = this.props.form
        // let patchVals = this.fillIncrementArray(56, 1)
        let patchVals = patchSizeValues
        let portVals = this.fillIncrementArray(16, 0)
        

        return formModal(
            form.opened,
            "Create fixture",
            "Amen",
            "No no no no",
            this,
            () => contentItem([
                rowItem([
                    errorItem(this.props.error != null, this.props.error)
                ]),
                rowItem([
                    textInputItem("Name", FORM_ID_NAME, "Fixture"),
                    textInputItem("Count", FORM_ID_COUNT, "1" )
                ]),
                rowItem([
                    selectItem("Patch type", FOMR_ID_PATCH, FixturePatch.all())
                ]),
                rowItem([
                    selectItem("Patch columns", FORM_ID_PATCH_COLS, patchVals),
                    selectItem("Patch rows", FORM_ID_PATCH_ROWS, patchVals)
                ]),
                rowItem([
                   <ModeItemList ref={this.modeItemList}/>
                ]),
                rowItem([
                    selectItem("Net", FORM_ID_PORT_NET, portVals),
                    selectItem("SubNet", FORM_ID_PORT_SUB, portVals),
                    selectItem("Universe", FORM_ID_PORT_UNI, portVals),
                    textInputItem("Address", FORM_ID_PORT_ADR, 1),
                    textInputItem("Util address", FORM_IOD_PROT_UTIL_ADR, 512)    
                ]),
                rowItem([
                    radioItem(
                        "Address auto increment", 
                        FORM_ID_ADR_INCREM, 
                        this.incrment)
                ])
            ])
        )
    }
}

const mapStateToProps = (state) => {
    return {
        form: state.fixtureFormReducer,
        error: state.formErrorReducer
    }
}

const mapDispatchToProps = (dispatch) => {
    return bindActionCreators({
        addFixture: addFixture,
        editFixture: editFixture,
        closeFixtureForm: closeFixtureForm,
        riseFormValueError: riseFormValueError,
        clearFormValueError: clearFormValueError,
        openModeManagerForm: openModeManagerForm
    }, dispatch)
}

export default connect(mapStateToProps, mapDispatchToProps)(FixtureForm)