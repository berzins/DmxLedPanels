import React, {Component} from 'react'
import {bindActionCreators} from 'redux'
import {connect} from 'react-redux'
import Modal from 'react-bootstrap4-modal' 
import { storeStateFormReducer, formErrorReducer } from '../reducers/formReducers'
import { closeStoreStateForm, riseFormValueError, MODE_NEW, MODE_EDIT } from '../actions/formActions'
import { saveState } from '../actions/stateActions'
import { isInteger } from '../util/util'

const FORM_ID_NAME = "FORM_STORE_ID_NAME"

class StoreStateForm extends Component {

    handleClick() {
        this.props.click.clicked = true
    }

    createTextInputItem(title, id, value) {
        return(
            <div className="form-group" key={id}>
            <label htmlFor={id}>{title}</label>
            <input type="text" className="form-control" id={id} placeholder={value} defaultValue={value}/>
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

    onClose(){
        this.props.closeStoreStateForm(this)
    }

    onSubmit() {
        const fn = this.getValue(FORM_ID_NAME)
        if(!(fn.length > 0)) {
            this.riseFormValueError("File name not set!")
        }
        this.props.saveState(fn)
        this.props.closeStoreStateForm(this)
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
        return(
            <Modal visible={form.opened} onClickBackdrop={() => this.onClose() } onClick={() => this.handleClick()}>
                <div className="modal-header">
                <h5 className="modal-title">{"Store state"}</h5>
                </div>
                <div className="modal-body">
                {this.renderError()}
                <form>
                <div className="form-row align-items-center">
                    {this.createRowItem([
                        this.createTextInputItem("File name", FORM_ID_NAME, "faking_fak"),
                    ])}
                </div>
                {}
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
        form: state.storeStateFormReducer,
        error: state.formErrorReducer
    }
}

const mapDispatchToProps = (dispatch) => {
    return bindActionCreators({
        saveState: saveState,
        closeStoreStateForm: closeStoreStateForm,
        riseFormValueError: riseFormValueError
    }, dispatch)
}

export default connect(mapStateToProps, mapDispatchToProps)(StoreStateForm)