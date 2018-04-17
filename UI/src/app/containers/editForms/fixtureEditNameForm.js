import React, {Component} from 'react'
import {bindActionCreators} from 'redux'
import {connect} from 'react-redux'
import Modal from 'react-bootstrap4-modal' 
import { fixtureEditNameFormReducer } from '../../reducers/formReducers'
import { closeEditFixtureNameForm } from '../../actions/formActions'
import { editFixtureName } from '../../actions/stateActions'
import store from '../../store'

const FORM_ID_NAME = "EDIT_FITURE_NAME_FORM"

class FixtureEditNameForm extends Component {

    constructor(props){
        super(props) 
        this.incrment = true
    }

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
        this.props.closeFixtureForm(this)
    }

    onSubmit() {

        let ids = store.getState().selectionReducer.fixtures
        let data = {
            name : this.getValue(FORM_ID_NAME)
        }
        
        this.props.editFixtureName(ids, data)
        this.props.closeEditFixtureNameForm(null)
    }



    getValue(id){
        return document.getElementById(id).value
    }




    render() {
        let form = this.props.form
        return(
            <Modal visible={form.opened} onClickBackdrop={() => this.onClose() }>
                <div className="modal-header">
                <h5 className="modal-title">Edit fixture name</h5>
                </div>
                <div className="modal-body">
                <form>
                <div className="form-row align-items-center">

                    {this.createRowItem([
                        this.createTextInputItem("Name", FORM_ID_NAME, "Fixture"),
                    ])}
                </div>
                </form>
                </div>
                <div className="modal-footer">
                <button type="button" className="btn btn-secondary" onClick={() => this.onClose() }>
                    Klancel
                </button>
                <button type="button" className="btn btn-primary" onClick={ () => this.onSubmit() }>
                    Edit
                </button>
                </div>
            </Modal>
        )
    }
}

const mapStateToProps = (state) => {
    return {
        form: state.fixtureEditNameFormReducer
    }
}

const mapDispatchToProps = (dispatch) => {
    return bindActionCreators({
        editFixtureName: editFixtureName,
        closeEditFixtureNameForm: closeEditFixtureNameForm
    }, dispatch)
}

export default connect(mapStateToProps, mapDispatchToProps)(FixtureEditNameForm)