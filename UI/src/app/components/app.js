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
                <div className= "col-12 btn-group">
                    <SaveButton className='' click={this.click}/>
                    <LoadButton className='btn-margin-right' click={this.click}/>
                    <DeselectButton className='btn-margin-right' click={this.click}/>

                </div>

                <div className= "col-12">
                <ItemContainer click={this.click}/>
                </div>
 
                <FixtureForm click={this.click}/>  
                <OutputForm click={this.click}/>
                <StoreStateForm click={this.click}/>  
                <LoadStateForm click={this.click}/>    
            </div>
        )
    }
}

export default App;