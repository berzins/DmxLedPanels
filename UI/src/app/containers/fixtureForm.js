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
    openModeManagerForm,
    submitModes,
    updateModes,
    setFixtureTemplate
} from '../actions/formActions'
import { addFixture, editFixture, getFixtureTemplates } from '../actions/stateActions'
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
    errorItem,
    textItem
} from './editForms/formItems'
import ModeItemList from './justItems/modeItemList'
import { 
    MODE_TYPE_ID, 
    MODE_COLUMNS_ID, 
    MODE_ROWS_ID
 } from './justItems/modeItem'
 import { getFixtureTemplateData, getValuesIndexPair } from './helpers/fixtureTemplateReader'
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
const FORM_ID_TEMPLATE = "FORM_ID_TEMPLATE"
const APPLY_TEMPLATE_BUTTON = "APPLY_TEMPLATE_BUTTON"

const DEFAULT_TEMPLATE = "Generic 1pix"


class FixtureForm extends Component {

    constructor(props){
        super(props) 
        this.incrment = true
        this.modeItemList = null
        this.state = {
            currentTemplate : DEFAULT_TEMPLATE,
            patchTypeIndex: 0,
            patchColumnIndex: 0,
            patchRowIndex: 0
        }
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

    selectTemplate() {
        this.props.setFixtureTemplate(this.getValue(FORM_ID_TEMPLATE))
    }

    getCurrentTemplate() {
        const prop = {...this.props}
        console.log("template props: ")
        console.log(prop);
        return prop.templates.data.find(t => {
            return t.Name == this.props.templates.current
        })
    }

    zeroIfLess(values) {
        return values.forEach(v => {v = v >= 0 ? v : 0})
    }

    getModeIndexies(uidata) {
        return uidata.modes.map(m => {
            return {
                typeIndex: m.name.index,
                colIndex: m.params[0].index,
                rowIndex: m.params[1].index
            }
        })
    }

    updateModes(uidata) {
        const modeIndexies = this.getModeIndexies(uidata)
        this.props.updateModes(modeIndexies)
        setTimeout(() => {
            this.props.submitModes(modeIndexies)
        }, 200)
    } 

    render() {
        let form = this.props.form
        let portVals = this.fillIncrementArray(16, 0)

        const templateNames = this.props.templates.data.map(t => {return t.Name})
        if(templateNames.length == 0) {
            this.props.getFixtureTemplates()
            return formModal(
                form.opened,
                "Create fixture",
                "Amen",
                "No no no no",
                this,
                () => contentItem([
                    rowItem([
                        textItem("Waiting data")
                    ])
                ])
            )
        }

        
        const template = this.getCurrentTemplate()
        const uidata = getFixtureTemplateData(template)
        
        if(uidata.modes.length > 0) {
            this.updateModes(uidata)
        }
        
        return formModal(
            form.opened,
            "Create fixture",
            "Amen",
            "No no no no",
            this,
            () => contentItem([
                rowItem([
                    selectItem("Templates", FORM_ID_TEMPLATE, templateNames, getValuesIndexPair(templateNames, this.props.templates.current).index),
                    buttonItem(APPLY_TEMPLATE_BUTTON, "Apply", () => this.selectTemplate())
                ]),
                rowItem([
                    errorItem(this.props.error != null, this.props.error)
                ]),
                rowItem([
                    textInputItem("Name", FORM_ID_NAME, uidata.name),
                    textInputItem("Count", FORM_ID_COUNT, "1" )
                ]),
                rowItem([
                    selectItem("Patch type", FOMR_ID_PATCH, uidata.pixelPatch.name.values, uidata.pixelPatch.name.index)
                ]),
                rowItem([
                    selectItem("Patch columns", FORM_ID_PATCH_COLS, uidata.pixelPatch.columns.values, uidata.pixelPatch.columns.index),
                    selectItem("Patch rows", FORM_ID_PATCH_ROWS, uidata.pixelPatch.rows.values, uidata.pixelPatch.rows.index)
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
        error: state.formErrorReducer,
        templates: state.fixtureTemplateReducer
    }
}

const mapDispatchToProps = (dispatch) => {
    return bindActionCreators({
        addFixture: addFixture,
        editFixture: editFixture,
        closeFixtureForm: closeFixtureForm,
        riseFormValueError: riseFormValueError,
        clearFormValueError: clearFormValueError,
        openModeManagerForm: openModeManagerForm,
        getFixtureTemplates: getFixtureTemplates,
        submitModes: submitModes,
        updateModes: updateModes,
        setFixtureTemplate: setFixtureTemplate
    }, dispatch)
}

export default connect(mapStateToProps, mapDispatchToProps)(FixtureForm)