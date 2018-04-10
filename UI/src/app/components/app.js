import React, { Component } from 'react';
import Fixture from '../containers/fixture'
import Output from '../containers/output'
import ItemContainer from '../containers/itemContainer'

class App extends Component { 
    render() {
        return(
            <div className="container-fluid">
                <ItemContainer/>
                
            </div>
        )
    }
}

export default App;