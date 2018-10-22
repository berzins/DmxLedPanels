import React, { Component } from 'react';
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

class App extends Component { 

    constructor(props) {
        super(props)
        this.click = {
            id: 0, 
            clicked: false
        }
    }


    handleClick() {
        this.click.clicked = false
    }

    render() {
        return(
            <div className="container-fluid">
            <DmxSignalItem click={this.click}/>
                <div className= "col-12 btn-group">
                    <SaveButton  click={this.click}/>
                    <LoadButton  click={this.click}/>
                    <ButtonUndoState click={this.click}/>
                    <ButtonRedoState click={this.click}/>
                    <HighlightButton click={this.click}/>
                    <DeselectButton click={this.click}/>

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
}

export default App;