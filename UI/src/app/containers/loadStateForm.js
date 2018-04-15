import React, {Component} from 'react'
import {bindActionCreators} from 'redux'
import {connect} from 'react-redux'
import Modal from 'react-bootstrap4-modal' 

import { 
    fileSelectedReducer, 
    loadStateFormReducer , 
    formErrorReducer 
} from '../reducers/formReducers'

import { 
    closeOpenStateForm, 
    riseFormValueError, 
    selectFile 
} from '../actions/formActions'

import { loadStateFromFile, getSavedStates } from '../actions/stateActions'
import { savedStatesReducer } from '../reducers/stateReducer'
import File from '../containers/file'

class LoadStateForm extends Component {



    handleClick() {
        this.props.click.clicked = true
    }
    
    createFileList(files) {
        return (
            <div className=" list-group" style={{width: 100+"%"}}>
                {files.names.map((f) => {
                    return(
                        <File key={f} filename={f.split('.')[0]} click={this.props.click}/>
                    )
                    })
                }
            </div>
        )
    }

    onClose(){
        this.props.selectFile(null)
        this.props.closeOpenStateForm(this)
    }

    onSubmit() {
        
        if(this.props.selectedFile.filename == null) {
            this.props.riseFormValueError("File not selected!")
            return
        }
        this.props.loadStateFromFile(this.props.selectedFile.filename)
        this.onClose()
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

    renderSelectedFile() {
        if(this.props.selectedFile.filename != null) {
            return <div className="badge badge-success">{"selected file: '" + this.props.selectedFile.filename + "'" }</div>
        } 
        return <div className="badge badge-danger">{"Wazzzzaaaaaaap!" }</div>
    }

    componentDidMount() {
        this.props.getSavedStates()
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

                    {this.createFileList(this.props.files)}
                    {this.renderSelectedFile()}
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
        selectedFile: state.fileSelectedReducer,
        files: state.savedStatesReducer,
        error: state.formErrorReducer,
        form: state.loadStateFormReducer,
    }
}

const mapDispatchToProps = (dispatch) => {
    return bindActionCreators({
        closeOpenStateForm: closeOpenStateForm,
        riseFormValueError: riseFormValueError,
        loadStateFromFile: loadStateFromFile,
        getSavedStates: getSavedStates,
        selectFile: selectFile
    }, dispatch)
}

export default connect(mapStateToProps, mapDispatchToProps)(LoadStateForm)