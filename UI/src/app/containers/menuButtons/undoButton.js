import React, { Component } from 'react'
import { bindActionCreators } from 'redux'
import { connect } from 'react-redux'
import { undoState } from '../../actions/stateActions'

class ButtonUndoState extends Component {

    handleClick() {
        this.props.click.clicked = true
        this.props.undoState()
    }

    render() {
        return(
            <div
            className={"button btn util-button btn-primary btn-margin-right "}
            autoComplete="off"
            onClick={ () => this.handleClick()}>
                <div><h3><b>Undo</b></h3></div>
            </div>
        );
    }
}


const mapDispatchToProps = (dispatch) => {
    return bindActionCreators({
        undoState: undoState
    }, dispatch)
}


export default connect(null,mapDispatchToProps)(ButtonUndoState);