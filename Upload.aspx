<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Upload.aspx.cs" Inherits="ShutDownDetails.Upload" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Shut Down Details</title>
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
    <script type = "text/javascript">
        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Do you want to change details, this will delete all previous details of selected month?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
     <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
    <asp:UpdateProgress runat="server" id="PageUpdateProgress" AssociatedUpdatePanelID="UpdatePanel2" DisplayAfter="0" >
        <ProgressTemplate>
            <div style="background-color: Gray; filter:alpha(opacity=60); opacity:0.60; width: 100%; top: 0px; left: 0px; position: fixed; height: 100%;">
            </div>
            <div style="margin:auto;font-family:Trebuchet MS;filter: alpha(opacity=100);opacity: 1;font-size:small;vertical-align: middle;top: 45%;position: fixed;
                right: 45%;color: #275721; text-align: center;background-color: White;height: 100px;">
                <table style=" background-color: White; font-family: Sans-Serif; text-align: center; border: solid 1px #275721; color: #275721; width: inherit; height: inherit; padding: 15px;">
                    <tr>
                        <td style=" text-align: inherit;">
                            <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/Loading.gif" Height="100px" Width="100px" />
                        </td>
                        <td style=" text-align: inherit;"><span style="font-family: Sans-Serif; font-size: medium; font-weight: bold; font">Please Wait...</span></td>
                    </tr>
                </table>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
     <asp:UpdatePanel ID="UpdatePanel2" runat="server">
      <ContentTemplate> 
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
     </ContentTemplate>
        </asp:UpdatePanel>
          </ContentTemplate>
        </asp:UpdatePanel>
        <div>
            <div id="wrapper">
		        <div class="shell">
			        <div class="container">							
				        <header class="header">
                            <h1 id="logo"><a href="#"></a></h1>                              
                            <nav id="navigation">
						        <ul>
							        <li class="active"><a href="logout.aspx">LogOut</a></li>							       						        
						        </ul>
					        </nav>					
                        </header>	
                        <br />
                        <br />
                        <br />
                        <br />            
                        <div style="border-width: thin; font-size: large; font-weight: bold; color: #800080; border-bottom-style: double; border-top-style: double; border-top-color: #0000FF; border-bottom-color: #FF0000;" align="center">
                            Unit
                            Shut Down Details
                        </div>
                        <br />                                   
                        <div align="center" style="width:100%;" >
                            Year - 
                            <asp:DropDownList ID="ddyear" runat="server" AutoPostBack="True" 
                                onselectedindexchanged="ddyear_SelectedIndexChanged">
                            </asp:DropDownList>     
                            Month - <asp:DropDownList ID="ddmonth" runat="server">
                            </asp:DropDownList>
                          &nbsp;&nbsp;&nbsp;
                            <asp:LinkButton ID="lnkbtn" runat="server" onclick="lnkbtn_Click">    View</asp:LinkButton>
                        </div>
                        <br />       
                        <div align="center" style="width:100%;" >
                            Site ID - &nbsp; 
                            <asp:DropDownList ID="ddsiteid" runat="server">
                                <asp:ListItem Selected="True" Value="0">&lt;-Select-&gt;</asp:ListItem>
                                <asp:ListItem Value="1">MR</asp:ListItem>
                                <asp:ListItem Value="2">VR</asp:ListItem>
                            </asp:DropDownList>  
                        </div>   

                        <br />       
                        
                        <div align="center" style="width:90%;margin-left:auto;margin-right:auto" >
                            <asp:GridView ID="resultgridview" runat="server" AutoGenerateColumns="False" 
                                CellPadding="4" ForeColor="#333333" Width="100%"                                 
                                ShowHeaderWhenEmpty="True">
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" 
                                        HorizontalAlign="Center" VerticalAlign="Middle" />                           
                                        <Columns>
                                            <asp:BoundField DataField="SITE_ID" HeaderText="SITE" ItemStyle-Width="50 px" />
                                            <asp:BoundField DataField="DATE_OIL_OUT" HeaderText="DATE (OIL OUT) (dd/mm/yyyy)" DataFormatString="{0:dd/MM/yy}" ItemStyle-Width="100 px"/>
                                            <asp:BoundField DataField="UNIT" HeaderText="UNIT" ItemStyle-Width="60 px" />
                                            <asp:BoundField DataField="SHUTDOWN" HeaderText="SHUTDOWN (hh:mm) (24Hr Format)" ItemStyle-Width="82 px"/>
                                            <asp:BoundField DataField="DATE_OIL_IN" HeaderText="DATE (OIL IN) (dd/mm/yyyy)" DataFormatString="{0:dd/MM/yy}" ItemStyle-Width="100 px" />
                                            <asp:BoundField DataField="START_UP" HeaderText="START UP (hh:mm) (24Hr Format)" ItemStyle-Width="82 px" />
                                            <asp:BoundField DataField="DURATION" HeaderText="DURATION [DAYS]" ItemStyle-Width="80 px" />
                                            <asp:BoundField DataField="REASONS" HeaderText="REASONS" ItemStyle-HorizontalAlign="Justify" />                     
                                        </Columns>
                                    <EditRowStyle BackColor="#999999" />
                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" HorizontalAlign="Center" 
                                        VerticalAlign="Middle" />
                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                            </asp:GridView>                                            
                        </div>
                        <div align="center" style="width:90%;margin-left:auto;margin-right:auto" >
                            <asp:TextBox ID="txtCopied" runat="server" TextMode="MultiLine" placeholder="Paste Here"
                            OnTextChanged="PasteToGridView" Height="150px" Width="300px" 
                            Visible="False" />
                        </div>
                        <br />
                        <br />
                        <div align="center" style="width:90%;margin-left:auto;margin-right:auto" >
                            <asp:Button ID="btncancel" runat="server" Text="Cancel" 
                                onclick="btncancel_Click" Visible="False"/>
                            
                            <asp:Button ID="btnreset" runat="server" Text="Reset" 
                                onclick="btnreset_Click" OnClientClick = "Confirm()"/>
                            
                            <asp:Button ID="btnsave" runat="server" Text="Submit" onclick="btnsave_Click" />                            
                        </div>  
                    </div>		
                </div> 
                <div class="footer">
				    <nav class="footer-nav">
					    <ul>
						    <li><a href="logout.aspx">Logout</a></li>						       
                        </ul>
                    </nav>
				    <p class="copy">Copyright &copy; 2016 All Rights Reserved. Design and Developed by HPCL Information Systems Center</a> </p>
                </div>                       
            </div>
        </div>
        
    </form>
</body>
</html>
