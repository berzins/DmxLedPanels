import React, { Component } from 'react'
import {bindActionCreators} from 'redux'
import {connect} from 'react-redux'
import { openFixtureForm, MODE_NEW, MODE_EDIT } from '../actions/formActions'


class AddFixtureButton extends Component {

    handleClick() {
        this.props.click.clicked = true
        this.props.openFixtureForm(MODE_NEW)
    }

    render() {
        return(
            <div
            className="button btn btn-success item-button" 
            autoComplete="off"
            onClick={() => this.handleClick()}
            >
                <div><b>Add Fixture</b></div>
            </div>
        );
    }
}

const mapStateToProps = (state) => {
    return {

    }
} 

const mapDispatchToProps = (dispatch) => {
    return bindActionCreators({
        openFixtureForm: openFixtureForm
    }, dispatch)
}

export default connect(mapStateToProps,mapDispatchToProps)(AddFixtureButton);