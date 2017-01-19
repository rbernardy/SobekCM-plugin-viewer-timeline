-- use YOURDATABASE;

-- Add TIMELINE as item view type
if ( not exists ( select 1 from SobekCM_Item_Viewer_Types where ViewType = 'TIMELINE' ))
begin
	
	insert into SobekCM_Item_Viewer_Types ( ViewType, [Order], DefaultView, MenuOrder )
	values ( 'TIMELINE', 1, 0, 98 );

end;
GO
