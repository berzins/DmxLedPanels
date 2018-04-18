import React, {Component} from 'react'
import {bindActionCreators} from 'redux'
import {connect} from 'react-redux'
import Modal from 'react-bootstrap4-modal' 
import { fixtureFormReducer, formErrorReducer } from '../reducers/formReducers'
import { closeFixtureForm, riseFormValueError, MODE_NEW, MODE_EDIT, clearFormValueError } from '../actions/formActions'
import { addFixture, editFixture } from '../actions/stateActions'
import { FixtureMode, FixturePatch, isInteger } from '../util/util'
import store from '../store'

const FORM_ID_NAME = "FORM_ID_NAME"
const FOMR_ID_PATCH = "FOMR_ID_PATCH"
const FORM_ID_PATCH_COLS = "FORM_ID_PATCH_COLS"
const FORM_ID_PATCH_ROWS = "FORM_ID_PATCH_ROWS"
const FORM_ID_MODE = "FORM_ID_MODE"
const FORM_ID_MODE_COLS = "FORM_ID_MODE_COLS"
const FORM_ID_MODE_ROWS = "FORM_ID_MODE_ROWS"
const FORM_ID_PORT_NET = "FORM_ID_PORT_NET"
const FORM_ID_PORT_SUB = "FORM_ID_PORT_SUB"
const FORM_ID_PORT_UNI = "FORM_ID_PORT_UNI"
const FORM_ID_PORT_ADR = "FORM_ID_PORT_ADR"
const FORM_ID_ADR_INCREM = "FORM_ID_ADR_INCREM"
const FORM_ID_COUNT = "FORM_ID_COUNT"


class FixtureForm extends Component {

    constructor(props){
        super(props) 
        this.incrment = true
    }

    handleClick() {
        this.props.click.clicked = true
    }

    chooseText(mode, newText, editText) {
        let title = ""
        switch(mode){
            case MODE_NEW: {
                return title =  newText
            }
            case MODE_EDIT: {
                return title = editText
            }
        }
    }

    getTitle(mode) {
        return this.chooseText(mode, "Create new fixtures", "Edit fixtures")
        
    }

    getSumbitTitle(mode) {
        return this.chooseText(mode, "Sumbit", "Edit")
    }

    createSelectItem(title, id, values) {

        return (
            <div 
            className="col-auto my-1" 
            key={id}
            onClick={() => this.handleClick() }
            >
            <label className="mr-sm-2" htmlFor="inlineFormCustomSelect">{title}</label>
            <select className="custom-select mr-sm-2" id={id}>
                {values.map((val, i) => {
                    return i === 0 ? 
                    <option value={val} key={title + val} defaultValue>{val}</option> : 
                    <option value={val} key={title + val}>{val}</option>
                    })
                }
            </select>
            </div>
        )
    }

    createTextInputItem(title, id, value) {
        return(
            <div className="form-group" key={id}>
            <label htmlFor={id}>{title}</label>
            <input type="text" className="form-control" id={id} placeholder={value} defaultValue={value}/>
            </div>
        )
    }

    createRadioItem(title, id, checked, callback) {
        let input = null
        if(checked) {
            input = <input className="form-check-input" type="checkbox" id={id} onChange={() => callback(this)} checked/>
        } else {
            input = <input className="form-check-input" type="checkbox" id={id} onChange={() => callback(this)} />
        }
        return(
            <div className="form-group" key={id}>
            <div className="form-check">
            {input}
            <label className="form-check-label" htmlFor="gridCheck">
                {title}
            </label>
            </div>
        </div>
        )
    }
    
    createRowItem(child) {
        return (
            <div className="col-12">
                <div className="form-row align-items-center">
                    {child.map((x) => {return x})}
                </div>    
            </div>
        )
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

    checkIntegerValue(start, end, name, value) {
        if(!isInteger(value)) {
            return{valid: false, error: name + " value is not an integer"}
        }
        if(value > end || value < start) {
            return {valid: false, error: name + " value should be in a range (1 ... 512)"}
        }
        return {valid: true, error: null};
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
            mode : this.getValue(FORM_ID_MODE),
            modeCol: this.getValue(FORM_ID_MODE_COLS),
            modeRow : this.getValue(FORM_ID_MODE_ROWS),
            net : this.getValue(FORM_ID_PORT_NET),
            sub : this.getValue(FORM_ID_PORT_SUB),
            uni : this.getValue(FORM_ID_PORT_UNI),
            addr : this.getValue(FORM_ID_PORT_ADR),
            increment : this.incrment
        }
        

        validatons.push(this.checkIntegerValue(1,512, "Address", data.addr))
        let res = this.validate(validatons)

        if(res !== "") {
            // we have form value error(s)
            this.props.riseFormValueError(res)
            return;
        }
        if(this.props.form.mode == MODE_NEW) {
            this.props.addFixture(count, data)    
        } else {
            this.props.editFixture(ids, data)          
        }
        this.props.clearFormValueError()
        this.props.closeFixtureForm(null)
    }



    getValue(id){
        return document.getElementById(id).value
    }

    renderError(){
        if(this.props.error !== null) {
            return(
                <div className="badge badge-danger">{this.props.error}</div>
            )
        }
        return(
            <div></div>
     
        )
    }

    render() {
        let form = this.props.form
        let patchVals = this.fillIncrementArray(30, 1)
        let portVals = this.fillIncrementArray(16, 0)
        let modeVals = [1, 3, 9]
        return(
            <Modal visible={form.opened} onClickBackdrop={() => this.onClose() }>
                <div className="modal-header">
                <h5 className="modal-title">{this.getTitle(form.mode)}</h5>
                </div>
                <div className="modal-body">
                {this.renderError()}
                <form>
                <div className="form-row align-items-center">

                    {this.createRowItem([
                        this.createTextInputItem("Name", FORM_ID_NAME, "Fixture"),
                        this.createTextInputItem("Count", FORM_ID_COUNT, 1)
                    ])}
                    {this.createRowItem([
                        this.createSelectItem("Patch type", FOMR_ID_PATCH, FixturePatch.all())
                    ])} 
                    {this.createRowItem([
                        this.createSelectItem("Patch columns", FORM_ID_PATCH_COLS, patchVals),
                        this.createSelectItem("Patch rows", FORM_ID_PATCH_ROWS, patchVals)
                    ])}
                    {this.createRowItem([
                        this.createSelectItem("Mode", FORM_ID_MODE, FixtureMode.all())
                    ])}
                    {this.createRowItem([
                        this.createSelectItem("Mode Columns", FORM_ID_MODE_COLS, modeVals),
                        this.createSelectItem("Mode Rows", FORM_ID_MODE_ROWS, modeVals)
                    ])}
                    {this.createRowItem([
                        this.createSelectItem("Net", FORM_ID_PORT_NET, portVals),
                        this.createSelectItem("SubNet", FORM_ID_PORT_SUB, portVals),
                        this.createSelectItem("Universe", FORM_ID_PORT_UNI, portVals),
                        this.createTextInputItem("Address", FORM_ID_PORT_ADR, 1)
                    ])}
                    {this.createRowItem([
                        this.createRadioItem(
                            "Address auto increment", 
                            FORM_ID_ADR_INCREM, 
                            this.incrment,
                            (form) => { form.incrment = !form.incrment } )
                    ])} 
                </div>
                </form>
                </div>
                <div className="modal-footer">
                <button type="button" className="btn btn-secondary" onClick={() => this.onClose() }>
                    Klancel
                </button>
                <button type="button" className="btn btn-primary" onClick={ () => this.onSubmit() }>
                    {this.getSumbitTitle(form.mode)}
                </button>
                </div>
            </Modal>
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
        clearFormValueError: clearFormValueError
    }, dispatch)
}

export default connect(mapStateToProps, mapDispatchToProps)(FixtureForm)