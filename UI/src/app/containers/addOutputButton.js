import React, { Component } from 'react'
import { bindActionCreators } from 'redux'
import { connect } from 'react-redux'
import {openOutputForm, MODE_NEW} from '../actions/formActions'

class AddOutputButton extends Component {

    handleClick() {
        this.props.click.clicked = true
        this.props.openOutputForm(MODE_NEW)
    }

    render() {
        return(
            <div
            className="button btn btn-success item-button" 
            autoComplete="off"
            onClick={() => this.handleClick()}
            >
                <div><b>Add Output</b></div>
            </div>
        );
    }
}

const mapStateToProps = (sate) => {
    return {

    }
}

const mapDispatchToProps = (dispatch) => {
    return bindActionCreators({
        openOutputForm: openOutputForm
    }, dispatch)
}

export default connect(mapStateToProps,mapDispatchToProps)(AddOutputButton);