import React, {Component} from 'react'
import {bindActionCreators} from 'redux'
import {connect} from 'react-redux'
import Fixture from '../containers/fixture'
import Output from '../containers/output'
import ItemContainer from '../containers/itemContainer'
import FixtureForm from '../containers/fixtureForm'
import OutputForm from '../containers/outputForm'
import SaveButton from '../containers/saveButton'
import LoadButton from '../containers/loadButton'
import StoreStateForm from '../containers/storeStateForm'
import LoadStateForm from '../containers/loadStateForm'
import DeselectButton from '../containers/deselectButton'
import HighlightButton from '../containers/hilightButton' 
import FixtureEditNameForm from '../containers/editForms/fixtureEditNameForm'
import FixtureEditAddressForm from '../containers/editForms/fixtureEditAddressForm'
import FixtureEditModeform from '../containers/editForms/fixtureEditModeForm'
import FixtureEditPatchFrom from '../containers/editForms/fixtureEditPatchForm'
import OutputEditNameForm from '../containers/editForms/outputEditNameForm'
import OutputEditPortForm from '../containers/editForms/outputEditPortForm'
import ButtonUndoState from '../containers/menuButtons/undoButton'
import ButtonRedoState from '../containers/menuButtons/redoButton'
import OutputEditIpForm from '../containers/editForms/outputEditIpForm'
import DmxSignalItem from '../containers/justItems/dmxSignalItem'
import ModeManagerForm from '../containers/forms/modeForm'
import ErrorForm from '../containers/forms/errorForm'

// modify buttons
import AddFixtureButton from '../containers/addFixtureButton'
import DeleteFixtureButton from '../containers/deleteFixtureButton'
import EditFixtureButton from '../containers/edditFixtureButton'
import EditOutputButton from '../containers/editOutputButton'
import AddOutputButton from '../containers/addOutputButton'
import DeleteOutputButton from '../containers/deleteOutputButton'

import { loggIn } from '../actions/stateActions'
import { sessionReducer } from '../reducers/stateReducer'
import { 
    contentItem,
    rowItem,
    textInputItem,
    textItem,
    buttonItem,
 } from '../containers/editForms/formItems'

 import { getFieldValue } from '../util/util'
 import { RemoteEventManager } from '../util/remoteEventManager'

 const ENTER_LOGIN = "enter_magic_field"


class App extends Component { 

    

    constructor(props) {
        super(props)
        this.click = {
            id: 0, 
            clicked: false
        }
        this.remoteEventManager = RemoteEventManager.getInstance()
    }

    handleClick() {
        this.click.clicked = false
    }

    onLoginPressed() {
        const password = getFieldValue(ENTER_LOGIN)
        this.props.loggIn(password)
    }

    getLoggingScreen() {
        return(
            contentItem([
                rowItem([
                    textInputItem("Enter magic", ENTER_LOGIN, "")
                ]),
                rowItem([
                    buttonItem("enter_magic_button", "Login", this.onLoginPressed.bind(this))
                ])
            ])
        )

    }

    getAppScreen() {
        const fixLeftOffest = {
            paddingLeft: '20px'
        }

        const btnContainerStyle = {
            border: 'solid',
            backgroundColor: 'rgb(25,25,25)'
        }

        const butotnStyle = {
            marginRight: '8px'
        }

        const gradient = {
            height: '3px',
            backgroundImage: 'linear-gradient(rgb(25,25,25), rgba(0,0,0,0.0))'
        }
        

        

        return(
            <div className="container-fluid">
                <div className="fixed-top">
                    <div style={btnContainerStyle}>
                    <DmxSignalItem click={this.click} />
                    <div className= "col-12 btn-group">
                        <SaveButton  click={this.click} style={butotnStyle}/>
                        <LoadButton  click={this.click} style={butotnStyle}/>
                        <ButtonUndoState click={this.click} style={butotnStyle}/>
                        <ButtonRedoState click={this.click} style={butotnStyle}/>
                        <HighlightButton click={this.click} style={butotnStyle}/>
                        <DeselectButton click={this.click} style={butotnStyle}/>
                    </div>

                    <div className="row" style={fixLeftOffest}>

                    <div className="col-6 col-md-8">
                    <div className="row">
                        <div className='col-12 btn-group'>
                                <AddOutputButton click={this.click}/>
                                <EditOutputButton click={this.click}/>
                                <DeleteOutputButton click={this.click}/>
                        </div>
                    </div>
                    </div>
                    <div className="col-6 col-md-4">
                    <div className="row">
                        <div className="col-12 btn-group">
                            <AddFixtureButton click={this.click}/>
                            <EditFixtureButton click={this.click}/>
                            <DeleteFixtureButton click={this.click}/>
                        </div>
                    </div> 
                    </div>
                    </div>
                    </div>
                    <div style={gradient}></div>
                </div>

                <div className= "col-12">
                <ItemContainer click={this.click}/>
                </div>
 
                <FixtureForm click={this.click}/>  
                <OutputForm click={this.click}/>
                <StoreStateForm click={this.click}/>  
                <LoadStateForm click={this.click}/>
                <FixtureEditNameForm click={this.click}/>
                <FixtureEditAddressForm click={this.click}/>  
                <FixtureEditModeform click={this.click}/>  
                <FixtureEditPatchFrom click={this.click}/>
                <OutputEditNameForm click={this.click}/>
                <OutputEditPortForm click={this.click}/>
                <OutputEditIpForm click={this.click}/>
                <ModeManagerForm click={this.click}/>
                <ErrorForm click={this.click}/>
                
            </div>
        )
    }

    render() {


        return (
            this.props.session.logged ? this.getAppScreen() : this.getLoggingScreen()
        )
    }
}

const mapStateToProps = (state) => {
    return {
        session: state.sessionReducer
    }
}

const matchDispatchToProps = (dispatch) => {
    return bindActionCreators({
        loggIn: loggIn
    }, dispatch)
}

export default connect(mapStateToProps, matchDispatchToProps)(App);