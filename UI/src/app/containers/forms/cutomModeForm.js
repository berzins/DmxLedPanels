import React, {Component} from 'react'
import {bindActionCreators} from 'redux'
import {connect} from 'react-redux'

class FixtureEditPatchFrom extends Component {

    onClose() {

    }

    onSubmit() {

    }

    render() {
        return(
            <div>THIS IS CUTOM MODE FROM</div>
        )
    }
}

const mapStateToProps = (state) => {
    return {
    }
}

const mapDispatchToProps = (dispatch) => {
    return bindActionCreators({
    }, dispatch)
}

export default connect(mapStateToProps, mapDispatchToProps)(FixtureEditPatchFrom)