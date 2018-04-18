import React, {Component} from 'react'
import {bindActionCreators} from 'redux'
import {connect} from 'react-redux'
import Modal from 'react-bootstrap4-modal' 
import { outputFormReducer, formErrorReducer } from '../reducers/formReducers'
import { 
    closeOutputForm, 
    riseFormValueError, 
    MODE_NEW, 
    MODE_EDIT,
    clearFormValueError
 } from '../actions/formActions'
import { addOutput } from '../actions/stateActions'
import { isInteger } from '../util/util'
import { 
    rowItem, 
    textInputItem, 
    selectItem, 
    radioItem,
    formModal,
    contentItem,
    errorItem
 } from './editForms/formItems'

 import { 
    fillIncrementArray, 
    checkIntegerValue,
    checkIpAddress,
    validate
    } from './editForms/formUtil'

const FORM_ID_NAME = "FORM_OUT_ID_NAME"
const FORM_ID_PORT_NET = "FORM_OUT_ID_PORT_NET"
const FORM_ID_PORT_SUB = "FORM_OUT_ID_PORT_SUB"
const FORM_ID_PORT_UNI = "FORM_OUT_ID_PORT_UNI"
const FORM_ID_ADR_INCREM = "FORM_OUT_ID_ADR_INCREM"
const FORM_ID_COUNT = "FORM_OUT_ID_COUNT"
const FORM_ID_IP = "FORM_OUT_IP_ADDRESS"

class OutputForm extends Component {

    constructor(props){
        super(props) 
        this.incrment = true
    }

    handleClick() {
        this.props.click.clicked = true
    }    

    onClose(){
        this.props.closeOutputForm(this)
    }

    onSubmit() {

        let validatons = [];
        let count = this.getValue(FORM_ID_COUNT)
        let data = {
            name : this.getValue(FORM_ID_NAME),
            net : this.getValue(FORM_ID_PORT_NET),
            sub : this.getValue(FORM_ID_PORT_SUB),
            uni : this.getValue(FORM_ID_PORT_UNI),
            ip: this.getValue(FORM_ID_IP),
            increment : this.incrment
        }
        
        validatons.push(checkIntegerValue(0, 1000, "Count", count))
        validatons.push(checkIpAddress(data.ip))
        let res = validate(validatons)
        if(res !== "") {
            // we have form value error(s)
            this.props.riseFormValueError(res)
            return;
        }
        
        this.props.addOutput(count, data)
        this.props.clearFormValueError()
        this.props.closeOutputForm(null)
    }



    getValue(id){
        return document.getElementById(id).value
    }

    render() {
        let form = this.props.form
        let portVals = fillIncrementArray(16, 0)
        const incr = this.incrment

        return(
            formModal(
                form.opened,
                "Create new output",
                "Just Do It",
                "Naah",
                this,
                () => contentItem([
                    rowItem([
                        errorItem(this.props.error !== null, this.props.error)
                    ]),
                    rowItem([
                        textInputItem("Name", FORM_ID_NAME, "Output"),
                        textInputItem("Count", FORM_ID_COUNT, 1)
                    ]),
                    rowItem([
                        selectItem("Net", FORM_ID_PORT_NET, portVals),
                        selectItem("SubNet", FORM_ID_PORT_SUB, portVals),
                        selectItem("Universe", FORM_ID_PORT_UNI, portVals)
                    ]),
                    rowItem([
                        textInputItem("IPAddress", FORM_ID_IP, "192.168.0.255")
                    ]),
                    rowItem([
                        radioItem(
                            "Address auto increment", 
                            FORM_ID_ADR_INCREM, 
                            incr,
                            (form) => { form.incrment = !form.incrment } )
                    ])
                ])
            )
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
        riseFormValueError: riseFormValueError,
        clearFormValueError: clearFormValueError,
    }, dispatch)
}

export default connect(mapStateToProps, mapDispatchToProps)(OutputForm)