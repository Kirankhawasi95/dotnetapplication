<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadValues.aspx.cs" Inherits="ShutDownDetails.UploadValues" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="css/style.css" type="text/css" media="all" />
    <script type="text/javascript">
        window.onload = function () {
            document.getElementById("<%=txtCopied.ClientID %>").onpaste = function () {
                var txt = this;
                setTimeout(function () {
                    __doPostBack(txt.name, '');
                }, 100);
            }
        };
    </script>
    <script type ="text/javascript" >
        function showButton() {
            document.getElementById('<%=btnupload.ClientID%>').style.display = "";
        }
</script>
</head>
<body>
    <form id="form1" runat="server">
        <div>           
            <div id="wrapper">
		        <div class="shell">
			        <div class="container">							
				        <header class="header">
                            <h1 id="logo"><a href="#"></a></h1>
                             
                            <nav id="navigation">
						        <ul>
							        <li class="active"><a href="#">LogOut</a></li>							       						        
						        </ul>
					        </nav>					
                        </header>	
                        <br />
                        <br />
                        <br />
                        <br />
                        <div style="border-width: thin; font-size: large; font-weight: bold; color: #800080; border-bottom-style: double; border-top-style: double; border-top-color: #0000FF; border-bottom-color: #FF0000;" align="center">
                            Shut Down Details
                        </div>
                        <br />                        
                        <div class="main" >
                          
                          
                         <div>               
            </div> 
            <br />
            <br />
            
            <div align="center">
                Upload New File Here ---->
                <asp:FileUpload ID="fileUpload1" runat="server" onchange="showButton()"/>                 
                <asp:LinkButton ID="btnupload" runat="server" onclick="btnupload_Click" 
                    style="display:none" xmlns:asp="#unknown">Upload</asp:LinkButton>
                <asp:Label ID="lbfilename" runat="server"></asp:Label>
            </div>
            <br />
            <br />            
            <div align="center">  
                <asp:CheckBox ID="chkpaste" runat="server" Text="Or Paste Values without column names" 
                    oncheckedchanged="chkpaste_CheckedChanged" AutoPostBack="True" />              
                <asp:TextBox ID="txtCopied" runat="server" TextMode="MultiLine" placeholder="Paste Here"
                    OnTextChanged="PasteToGridView" Height="150px" Width="300px" 
                    Visible="False" />
            </div>
            <div align="center">
                    <asp:LinkButton ID="lnkbuttonsample" runat="server" 
                        onclick="lnkbuttonsample_Click">Download Sample File</asp:LinkButton>
            </div>
                        </div>	
			        </div>
                   		
			        <div class="footer">
				        <nav class="footer-nav">
					        <ul>
						        <li><a href="#">Home</a></li>						       
					        </ul>
				        </nav>
				        <p class="copy">Copyright &copy; 2016 All Rights Reserved. Design and Developed by HPCL Information Systems Center</a> </p>
			        </div>
		        </div>		
	        </div>        
        </div>
    </form>
</body>
</html>
