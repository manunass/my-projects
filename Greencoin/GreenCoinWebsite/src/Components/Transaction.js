import React,{useState,useEffect} from 'react'
import {Table} from 'react-bootstrap'
import { MDBCard} from
'mdbreact'
import axios from 'axios'
import {baseUrl} from '../../package.json'


export default function Transaction({match}) {
    const [transaction, settransaction] = useState({ Id: "", TransferId: "", UnixTime: 0, Description:"",PreAmount:0,Amount:0,PostAmount:0 })
  
    const [update, setupdate] = useState(true)
    useEffect( () => {
     console.log(match)
        const getTransaction = async () => {
            
            try {
               
          const tran = await axios.get(baseUrl+'/api/Wallets/'+match.params.id+'/transactions')
             
              settransaction(tran.data.Transactions);
              setupdate(false)
             
                
            } catch (err) {
              console.error(err.message);
            }
          };
        
          getTransaction();
       
       
      }, []);
    return (
        <div>
           <div class="d-flex justify-content-between p-2"> 
            <div className="text-dark">Transaction List</div>
            < div className="text-info" > Wallet ID:{match.params.id} </div>
           </div>
         
           
            <MDBCard >
            <Table responsive>
                <thead className="wallet">
                <tr>
                <th>ID</th>
               
                <th>Transfer ID</th>
                <th>UnixTime</th>
                <th>Description</th>
                <th>Pre Amount</th>
                <th>Amount</th>
                <th>Post Amount</th>
                </tr>
               </thead>
               {!update ?
               <tbody>
                  
               {transaction.map(bag =>{
                return(  
                <tr key={bag.Id}>
                      <td>{bag.Id}</td>
                    
                      <td>{bag.TransferId}</td>
                      <td>{bag.UnixTime}</td>
                      <td>{bag.Description}</td>
                      <td>{bag.PreAmount}</td>   
                      <td>{bag.Amount}</td>
                      <td>{bag.PostAmount}</td>
                 </tr>
                 ) })} 
                
               </tbody>
                : " "}
                </Table>
                </MDBCard>
        </div>
    )
}
