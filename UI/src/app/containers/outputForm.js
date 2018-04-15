import React, {Component} from 'react'
import {bindActionCreators} from 'redux'
import {connect} from 'react-redux'
import Modal from 'react-bootstrap4-modal' 
import { outputFormReducer, formErrorReducer } from '../reducers/formReducers'
import { closeOutputForm, riseFormValueError, MODE_NEW, MODE_EDIT } from '../actions/formActions'
import { addOutput } from '../actions/stateActions'
import { isInteger } from '../util/util'

const FORM_ID_NAME = "FORM_OUT_ID_NAME"
const FORM_ID_PORT_NET = "FORM_OUT_ID_PORT_NET"
const FORM_ID_PORT_SUB = "FORM_OUT_ID_PORT_SUB"
const FORM_ID_PORT_UNI = "FORM_OUT_ID_PORT_UNI"
const FORM_ID_ADR_INCREM = "FORM_OUT_ID_ADR_INCREM"
const FORM_ID_COUNT = "FORM_OUT_ID_COUNT"

class OutputForm extends Component {

    constructor(props){
        super(props) 
        this.incrment = true
    }

    handleClick() {
        this.props.click.clicked = true
    }

    getTitle(mode) {
        let title = ""
        switch(mode){
            case MODE_NEW: {
                return title = "Create new outputs" 
            }
            case MODE_EDIT: {
                return title = "Edit outputs"
            }
        }
    }

    createSelectItem(title, id, values) {

        return (
            <div className="col-auto my-1" key={id}
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
        this.props.closeOutputForm(this)
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
        let count = this.getValue(FORM_ID_COUNT)
        let data = {
            name : this.getValue(FORM_ID_NAME),
            net : this.getValue(FORM_ID_PORT_NET),
            sub : this.getValue(FORM_ID_PORT_SUB),
            uni : this.getValue(FORM_ID_PORT_UNI),
            increment : this.incrment
        }
        
        validatons.push(this.checkIntegerValue(0, 1000, "Count", count))
        let res = this.validate(validatons)
        if(res !== "") {
            // we have form value error(s)
            this.props.riseFormValueError(res)
            return;
        }
        
        this.props.addOutput(count, data)
        this.props.closeOutputForm(null)
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
        let portVals = this.fillIncrementArray(16, 0)
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
                        this.createTextInputItem("Name", FORM_ID_NAME, "Output"),
                        this.createTextInputItem("Count", FORM_ID_COUNT, 1)
                    ])}
                    {this.createRowItem([
                        this.createSelectItem("Net", FORM_ID_PORT_NET, portVals),
                        this.createSelectItem("SubNet", FORM_ID_PORT_SUB, portVals),
                        this.createSelectItem("Universe", FORM_ID_PORT_UNI, portVals)
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
                    Whaatoijfjainjowila?
                </button>
                <button type="button" className="btn btn-primary" onClick={ () => this.onSubmit() }>
                    Submit
                </button>
                </div>
            </Modal>
        )
    }
}

const mapStateToProps = (state) => {
    return {
        form: state.outputFormReducer,
        error: state.formErrorReducer
    }
}

const mapDispatchToProps = (dispatch) => {
    return bindActionCreators({
        addOutput,
        closeOutputForm: closeOutputForm,
        riseFormValueError: riseFormValueError
    }, dispatch)
}

export default connect(mapStateToProps, mapDispatchToProps)(OutputForm)